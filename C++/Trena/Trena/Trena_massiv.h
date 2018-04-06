#include <iostream>
#include <ctime>
using namespace std;
struct peremennay
{
	int razmer_mass;
	int *massiv,max,s_max,min,s_min,*odnomer_massiv;
	
};
peremennay perem;
void zapol()
{
	
	if (perem.razmer_mass <= 0)
	{
		perem.razmer_mass = rand() % 10;
		cerr << "" << endl;
	}
	
};
class Massivi
{
public:void massivis()
{
	setlocale(LC_ALL, "");
	srand(time(0));
	zapol();
	perem.massiv = new int[perem.razmer_mass];
	perem.odnomer_massiv = new int[perem.razmer_mass];
	
	for (int i = 0; i < perem.razmer_mass; i++)
	{
		for (int j = 0; j < perem.razmer_mass; j++)
		{
			perem.massiv[i, j] = rand() % 100;
			cout <<"\t"<< perem.massiv[i, j]<<" ";
		}
		cout << endl;
	}
	perem.max = perem.s_max = perem.massiv[0];
	for (int i = 0; i < perem.razmer_mass; i++)
	{
		for (int j = 0; j < perem.razmer_mass; j++)
		{
			if (perem.massiv[i, j] > perem.max)
			{
				perem.max = perem.massiv[i, j];
			}
		}
	}
	cout << "\nМаксимальный элемент в массиве: " << perem.max<<"\n";
	perem.min = perem.s_min = perem.massiv[0];
	for (int i = 0; i < perem.razmer_mass; i++)
	{
		for (int j = 0; j < perem.razmer_mass; j++)
		{
			if (perem.massiv[i, j] < perem.min)
			{
				perem.min = perem.massiv[i, j];
			}
		}
	}
	cout << "Минемальное число в массиве: " << perem.min<<"\n";
	cout << "\n"<<"___________________________________________________________________________________________________";
	cout << endl;
	cout << "Одномерный массив: ";
	for (int i = 0; i < perem.razmer_mass; i++)
	{
		perem.odnomer_massiv[i] = rand() % 100;
		cout << perem.odnomer_massiv[i] <<" ";
	}
	cout << endl;
	int temp;
	for (int i = 0; i < perem.razmer_mass - 1; i++)
	{
		for (int j = 0; j < perem.razmer_mass - i - 1; j++)
		{

			if (perem.massiv[j] == perem.odnomer_massiv[j + 1])
			{
				temp = perem.massiv[j];
				perem.massiv[j] = perem.odnomer_massiv[j + 1];
				perem.odnomer_massiv[j + 1] = temp;
			}
		}
	}
	cout << "Отсортированный массив: ";
	for (int i = 0; i < perem.razmer_mass; i++)
	{
		cout<<perem.odnomer_massiv[i]<<" ";
	}
	cout << endl;
	delete []perem.odnomer_massiv;
	cout << "Время работы программы:  " << clock() / 1000.0 << endl;
	system("pause");
}
};


