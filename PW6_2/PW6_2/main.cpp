#include <iostream>
#include <fstream>
#include <vector>
#include <iterator>
//#include <CL/cl.h>

#include <CL/cl.hpp>
#include <vector>
#include <random>
#include <ctime>
#include <utility>
#include <cstdio>
#include <cstdlib>
#include <cstdio>






void randomInit(cl_int * dane,int maxNumber, int size)
{
	for(int i = 0; i < size ; i++)
	{
		dane[i] = rand() % maxNumber ;
	}

}



inline void checkErr(cl_int err, const char* name)
{
	if(err != CL_SUCCESS)
	{
		std::cerr<<"ERROR: " << name << " (" << err << ")" <<std::endl;
		std::cin.get();
		exit(EXIT_FAILURE);
	}
}

int main(){
	srand(time(NULL));

	cl_int err;
	std::vector<cl::Platform> platofrmList;

	cl::Platform::get(&platofrmList);
	std::cerr<<"Platform number is: "<<platofrmList.size()<<std::endl;


	cl_context_properties cprops[3] ={
		CL_CONTEXT_PLATFORM,
		(cl_context_properties) (platofrmList[0])(),
		0
	};


	cl::Context context(CL_DEVICE_TYPE_GPU,cprops,NULL,NULL,&err);

	const int matrixSize = 10;
	const int maxValue = 10;

	int** matrixA = new int*[matrixSize];
	int** matrixB = new int*[matrixSize];
	int** matrixOut = new int*[matrixSize];

	std::cout<<std::endl<<"Macierz A"<<std::endl<<std::endl;

	for(int i = 0; i < matrixSize; i++)
	{
		matrixA[i] = new int[matrixSize];
		randomInit( matrixA[i], maxValue, matrixSize);
		
		for(int j = 0 ; j <matrixSize; j++)
		{
			std::cout<<matrixA[i][j]<<" ";
		}
		std::cout<<std::endl;
	}

	std::cout<<std::endl<<"Macierz B"<<std::endl<<std::endl;

	for(int i = 0; i < matrixSize; i++)
	{
		matrixB[i] = new int[matrixSize];
		randomInit( matrixB[i], maxValue, matrixSize);

		for(int j = 0 ; j <matrixSize; j++)
		{
			std::cout<<matrixB[i][j]<<" ";
		}
		std::cout<<std::endl;
	}

	for(int i = 0; i < matrixSize; i++)
	{
		matrixOut[i] = new int[matrixSize];
		for(int j = 0 ; j <matrixSize; j++)
		{
			matrixOut[i][j] = 0;
		}
	}


	cl::Buffer  inMatrixA[matrixSize];
	cl::Buffer  inMatrixB[matrixSize];
	cl::Buffer  outMatrix[matrixSize];


	for(int i = 0 ; i < matrixSize ; i++)
	{
		inMatrixA[i] =  cl::Buffer(context,CL_MEM_READ_ONLY,sizeof(int) * matrixSize);
	}

	for(int i = 0 ; i < matrixSize ; i++)
	{
		inMatrixB[i] =  cl::Buffer(context,CL_MEM_READ_ONLY,sizeof(int) * matrixSize);
	}

	for(int i = 0 ; i < matrixSize ; i++)
	{
		outMatrix[i] =  cl::Buffer(context,CL_MEM_READ_WRITE,sizeof(int) * matrixSize);
	}

	std::vector<cl::Device> devices;
	devices = context.getInfo<CL_CONTEXT_DEVICES>();
	checkErr(devices.size() > 0 ? CL_SUCCESS : -1, "devices.size() > 0");

	std::ifstream file("mnozenie.cl");

	checkErr(file.is_open() ? CL_SUCCESS : -1, "mnozenie.cl");
	std::string linia;
	std::getline(file,linia);
	std::cout<<linia;

	std::string prog(std::istreambuf_iterator<char>(file), (std::istreambuf_iterator<char>()));

	cl::Program::Sources source(1,std::make_pair(prog.c_str(),prog.length() ));


	cl::Program program=cl::Program(context,source);
	err = program.build(devices);
	checkErr(err,"Program.build()");

	cl::CommandQueue queue(context, devices[0], 0, &err);
	checkErr(err,"CommandQueue::CommandQueue()");

	for(int i = 0 ; i <matrixSize ; i++)
	{
		queue.enqueueWriteBuffer(inMatrixA[i],CL_TRUE,0,sizeof(int) * matrixSize, matrixA[i]);
	}

	for(int i = 0 ; i <matrixSize ; i++)
	{
		queue.enqueueWriteBuffer(inMatrixB[i],CL_TRUE,0,sizeof(int) * matrixSize, matrixB[i]);
	}

	for(int i = 0 ; i <matrixSize ; i++)
	{
		queue.enqueueWriteBuffer(outMatrix[i],CL_TRUE,0,sizeof(int) * matrixSize, matrixOut[i]);
	}
	
	cl::Kernel kernels[matrixSize];

	for(int i = 0; i < matrixSize ; i++)
	{
		kernels[i] = cl::Kernel(program,"multiply",&err);

		kernels[i].setArg(0,inMatrixA[i]);
		kernels[i].setArg(1,inMatrixB[i]);
		kernels[i].setArg(2,outMatrix[i]);
		kernels[i].setArg(3,matrixSize);

	}

	cl::Event event;

	for(int i = 0 ; i < matrixSize; i++)
	{
		err = queue.enqueueNDRangeKernel(kernels[i], cl::NullRange,cl::NDRange(matrixSize),cl::NDRange(1),NULL,&event); 
		checkErr(err,"CommandQueue::enqeueNDRangeKernel()");
	}

	event.wait();

	for(int i = 0 ; i < matrixSize ; i++)
	{
		err = queue.enqueueReadBuffer(outMatrix[i], CL_TRUE, 0, sizeof(int) * matrixSize, matrixOut[i]);
		checkErr(err, "CommandQueue::enqueueReadBuffer()");
	}
	
	std::cout<<std::endl<<"Macierz A * Macierz B"<<std::endl<<std::endl;

	for(int i = 0; i < matrixSize; i++)
	{
		for(int j = 0 ; j <matrixSize; j++)
		{
			std::cout<<matrixOut[i][j]<<" ";
		}
		std::cout<<std::endl;
	}

	for(int i = 0; i < matrixSize; i++)
	{
		delete[] matrixA[i];
	}

	for(int i = 0; i < matrixSize; i++)
	{
		delete[] matrixB[i];
	}

	for(int i = 0; i < matrixSize; i++)
	{
		delete[] matrixOut[i];
	}

	delete[] matrixA;
	delete[] matrixB;
	delete[] matrixOut;

	return 0;
}