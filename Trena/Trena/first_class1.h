#include <iostream>

using namespace std;
class First {
public: void first_class()
{
	for (int i = 1; i < 10; i++)
	{
		for (int j = 1; j < 10; j++)
		{
			cout << i * j << "\t";
		}
		cout << endl;
	}
}
};

