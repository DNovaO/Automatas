#include <stdio.h>
#include <math.h>
#include <iostream>

char altura,i,j,k;

void main() // Funcion principal
{
    for (i=0; i<10; i++)
    {
        printf("\nHola");
        k = i;
    }
    // k = (int)(10);
    // k = 1.5;
    // k = (int)((char)(1.5));
    /**
    k = (3+5)*8-(10-4)/2; // 61

    if (1==1)
    {
        if (k == 61)
        {
            k = 3;
        }
        k = 2;
    }
    */
    //*8 - (10-4)/2;

    /*
    printf("\nAltura: ");
    scanf("&i",&altura);

    printf("\nfor:\n");
    for (i = 1; i <= altura; i++)
    {
        for (j = 250; j < 250+i; j++)
        {
            if (j%2==0)
                printf("-");
            else
                printf("+");
        }
        printf("\n");
    }
    printf("\nwhile:\n");
    i = 1;
    while (i <= altura)
    {
        j = 250;
        while (j < 250+i)
        {
            if (j%2==0)
                printf("-");
            else
                printf("+");
            j++;
        }
        i++;
        printf("\n");
    }
    printf("\ndo:\n");
    i = 1;
    do
    {
        j = 250;
        do
        {
            if (j%2==0)
                printf("-");
            else
                printf("+");
            j++;
        } while (j < 250+i);
        i++;
        printf("\n");
    } while (i <= altura);
    */
}
