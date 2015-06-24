#include <iostream>
#include <string>
#include <algorithm>
#include <thread>
#include <mutex>


class Permutacja{

	std::string alfabet;
	std::string haslo;
	int n;

public:
	void haslo_init(){
		if(alfabet.length() == n){
			haslo = alfabet;
		}else{
			int roznica = n - alfabet.length();
			std::string koniec;
			for(int i = 0 ; i < roznica ; i++){
				//haslo.push_back(alfabet.c_str()[0]);
				koniec += alfabet[0];
				//std::cout<<alfabet<<std::endl;
				haslo = alfabet + koniec;
			}

		}
		std::sort(begin(haslo),end(haslo));
	}

	bool haslo_next(){
		bool wynik = std::next_permutation(begin(haslo),end(haslo));
		return wynik;
	}

	std::string getHaslo(){
		return haslo;
	}

	void setAlfabet(std::string alfabet){
		this->alfabet = alfabet;
	}

	Permutacja(std::string alfabet){
		this->alfabet = alfabet;
		n = alfabet.length();
	}

	Permutacja(std::string alfabet, int n){
		this->alfabet = alfabet;
		this->n = n;
	}
};

class Buffor
{
	std::string zawartoscBufora;

public:

	std::mutex blokada;
	bool czyPelny;
	
	void dodajDoBuffora(std::string haslo){
		zawartoscBufora = haslo;
	}

	std::string pobierzBuffora(){

		return zawartoscBufora;
	}

	Buffor(){
		czyPelny = false;

	}
};

class Producent 
{
	Buffor * buffor;
	Permutacja * permutacja;
	std::string haslo;

public:
	Producent(Buffor * buffor,Permutacja * permutacja){
		this->buffor = buffor;
		this->permutacja = permutacja;
	}

	void run(){
		permutacja->haslo_init();
		do{
			haslo = permutacja->getHaslo();
			std::cout<<"p: "<<haslo<<std::endl;

			while(buffor->czyPelny == true){

			}

			buffor->blokada.lock();
			buffor->dodajDoBuffora(haslo);
			buffor->czyPelny = true;
			buffor->blokada.unlock();


		}while(permutacja->haslo_next());

	}

};

class Konsument
{
	Buffor * buffor;
	std::string haslo;
	std::string hasloKlucz;

public:
	Konsument(Buffor * buffor,std::string hasloKlucz){
		this->buffor = buffor;
		this->hasloKlucz = hasloKlucz;
	}

	void run(){
		while(true){

			while(buffor->czyPelny == false){

			}

			buffor->blokada.lock();
			haslo = buffor->pobierzBuffora();
			buffor->czyPelny = false;
			buffor->blokada.unlock();
			std::cout<<"k: "<<haslo<<std::endl;
			if(haslo.compare(hasloKlucz) == 0){
				break;
			}
		}

	}

};




int main(){
	std::string alfabet = "abcd";
	Permutacja permutacja(alfabet);

	permutacja.haslo_init();
	std::cout<<permutacja.getHaslo()<<std::endl;

	while(permutacja.haslo_next()){
		std::cout<<permutacja.getHaslo()<<std::endl;
	}

	std::cout<<"----------------------------------------"<<std::endl;


	Permutacja permutacja2(alfabet,4);
	permutacja2.haslo_init();
	std::cout<<permutacja2.getHaslo()<<std::endl;

	while(permutacja2.haslo_next()){
		std::cout<<permutacja2.getHaslo()<<std::endl;
	}

	Buffor buffor;

	Producent producent(&buffor,&permutacja);
	std::string hasloKlucz = "bacd";
	Konsument konsument(&buffor,hasloKlucz);

	std::thread watekProducenta(&Producent::run,producent);
	std::thread watekKonsumenta(&Konsument::run,konsument);

	if(watekKonsumenta.joinable()){
		watekProducenta.detach();
		watekKonsumenta.join();
	}



	return 0;
}