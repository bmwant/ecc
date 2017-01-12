using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EllipticCurveCryptography
{
    public class PereborVariantov
    {
        int startCount;
        int elementsCount; // Общее число элементов
        int cnt; // количество,  с которого начать перебор элементов
        int maxcnt; // max кол-во одновременно перебираемых элементов
        int[] arr; // массив для хранения текущих перебираемых номеров 
        int curQty; // текущее количество элементов для перебора
        int curIteration; // текущая итерация для текущего значения curQty
        // int curMaxIterations;  // максимально возможное число итераций ля текущего curQty
        Boolean sw;
        int tv; // точка внимания

        // Конструктор
        // Например перебрать все варианты в 5 из 36 так: PereborVariantov(5, 36)
        public PereborVariantov(int qty, int startValue, int totalCount) : this(qty, qty, startValue, totalCount) { }

        // Например перебрать все варианты (3 из 36), (4 из 36), (5 из 36) так: PereborVariantov(3, 5, 36)
        public PereborVariantov(int minQty, int maxQty, int startValue, int totalCount)
        {
            if ((minQty < 1) || (minQty > maxQty) || (maxQty > totalCount))
            {// Генерим исключение
                throw new IndexOutOfRangeException();
            }
            cnt = minQty;
            maxcnt = maxQty;
            elementsCount = totalCount;
            curQty = cnt;

            arr = new int[0];
            int tv = curQty - 1; // точка внимания (инициализация) = количество перебираемых элементов -1 (поправка на 0-е начало массива
            sw = true;
            startCount = startValue;
        }

        // Меняет максимальное число перебираемых элементов (можно использовать наряду с GetNext)
        public void SetMaxCount(int mx)
        {
            maxcnt = mx;
        }

        public void getCurStat(out int cIter, out int curNum)
        {
            cIter = curIteration;
            curNum = curQty;
        }

        public void getCurStat(out int cIter)
        {
            cIter = curIteration;
        }

        public int Verojatnost(int aa, int bb)
        {
            ulong a = (ulong)aa;
            ulong b = (ulong)bb;
            ulong n1 = 1;
            ulong n2 = 1;
            for (ulong i = b - a + 1; i <= b; i++) n1 *= i;
            for (ulong i = 1; i <= a; i++) n2 *= i;
            ulong res = n1 / n2;
            return (int)res;
        }

        // Переключает на начало следующего уровня количества перебираемых элементов, прерывая текущий уровень
        public void SwitchToNextLevel()
        {
            curQty++;
            arr = new int[0];
            int tv = curQty - 1; // точка внимания (инициализация) = количество перебираемых элементов -1 (поправка на 0-е начало массива
            sw = true;
        }

        // Возращает следуюшую партию элементов из вариантов перебора
        public Boolean GetNext(out int[] myArr) // out
        {
            for (; curQty <= maxcnt; curQty++, sw = true) // пройти по всем размерностям
            {
                if (arr.Length != curQty)
                {
                    Array.Resize(ref arr, curQty);
                    for (int j = 0; j < curQty; j++) arr[j] = j + startCount; // инициализация номеров для перебора
                    tv = curQty - 1;
                    curIteration = 0; // Обнуляем текущую итерацию
                }

                while (true) // Большой цикл обработки
                {
                    if (sw)
                    {
                        sw = false;
                        myArr = new int[arr.Length];
                        Array.Copy(arr, myArr, arr.Length);
                        curIteration++; // увеличиваем счетчик итераций

                        return true;
                        // выполняемая функция для текущего значения элементов в массиве arr
                        // -> вставляем сюда <-
                    }

                    if (arr[tv] <= elementsCount - curQty + tv)
                    {
                        arr[tv]++;
                        // теперь все элеиенты которые были справа лепим к текущему элементу
                        for (int m = tv + 1; m < curQty; m++)
                        {
                            arr[m] = arr[m - 1] + 1;
                        }
                        tv = curQty - 1;
                        sw = true;
                    }
                    else
                    {
                        tv--;
                        sw = false;
                        if (tv < 0) break;
                    }
                }
            }
            myArr = new int[0];
            return false;
        }
    } 
}
