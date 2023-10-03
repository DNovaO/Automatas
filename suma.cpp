#include <stdio.h>
#include <math.h>
#include <iostream>

char altura, i, j;
char a;
int b, e, d;

void main() // Funcion principal
{

    b = 0;
    while (b != 5)
    {
        printf("Hola mundo.\n");
        b++;
    }

    printf("-------- Aqui entramos a la piramide ----------------\n");
    
    printf("\nAltura: ");
    scanf("&i",&altura);

    for (i = 1; i <= altura; i++)
    {
        for (j = 1; j <= i; j++)
        {
            if (j%2==0)
                printf("-");
            else
                printf("+");
        }
        printf("\n"); 
    }
    printf("saliendo de la piramide\n");

    printf("--------Aqui entramos al do/while ----------------\n");

    e = 0;
    do
    {
        printf("Adios Mundo.\n");
        e++;
    } while (e < 10);

    do
    {
        printf("¿Deseas continuar? (Ingresa 1 para continuar, otro valor para salir)\n");
        scanf("%d", &d);
    } while (d == 1);

    printf("Salimos de todo con exito =) ");

}
