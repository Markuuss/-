#include <iostream>
#include "first_class1.h"
#include "Trena_massiv.h"
using namespace std;
void vibor_lesson() {
	cout << "Выбери урок: ";
	int vibor = 0;
	cin >> vibor;

	switch (vibor)
	{
	case 1:

		First first;
		first.first_class();
		vibor_lesson();
		break;
	case 2:
		Massivi mass;
		mass.massivis();
		vibor_lesson();
		break;
	case 3:

		vibor_lesson();
		break;
	default:
		break;
	}
};
int main()
{
	setlocale(LC_ALL, "");
	vibor_lesson();
	system("pause");
	return 0;
}
