#include <stdio.h>

int main(){
	int x = 1;
	do{
		printf("%2d\n", x++);
	} while (x--);
	system("pause");
	return 0;
}
