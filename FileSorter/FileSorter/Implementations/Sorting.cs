using System;
using System.Linq;
using FileSorter.Models;

namespace FileSorter.Implementations
{
    public static class Sorting
    {
        //метод обмена элементов
        private static void Swap(ref SeparatedLine e1, ref SeparatedLine e2)
        {
            var tmp = e1;
            e1 = e2;
            e2 = tmp;
        }

        //сортировка пузырьком
        public static SeparatedLine[] BubbleSort(SeparatedLine[] input)
        {
            var array = new SeparatedLine[input.Length];
            Array.Copy(input, array, input.Length);
            var len = array.Length;
            for (var i = 1; i < len; i++)
            for (var j = 0; j < len - i; j++)
                if (array[j] > array[j + 1])
                    Swap(ref array[j], ref array[j + 1]);

            return array;
        }

        //сортировка перемешиванием
        public static SeparatedLine[] ShakerSort(SeparatedLine[] input)
        {
            var array = new SeparatedLine[input.Length];
            Array.Copy(input, array, input.Length);
            for (var i = 0; i < array.Length / 2; i++)
            {
                var swapFlag = false;
                //проход слева направо
                for (var j = i; j < array.Length - i - 1; j++)
                    if (array[j] > array[j + 1])
                    {
                        Swap(ref array[j], ref array[j + 1]);
                        swapFlag = true;
                    }

                //проход справа налево
                for (var j = array.Length - 2 - i; j > i; j--)
                    if (array[j - 1] > array[j])
                    {
                        Swap(ref array[j - 1], ref array[j]);
                        swapFlag = true;
                    }

                //если обменов не было выходим
                if (!swapFlag)
                    break;
            }

            return array;
        }


        //сортировка вставками
        public static SeparatedLine[] InsertionSort(SeparatedLine[] input)
        {
            var array = new SeparatedLine[input.Length];
            Array.Copy(input, array, input.Length);
            for (var i = 1; i < array.Length; i++)
            {
                var key = array[i];
                var j = i;
                while (j > 1 && array[j - 1] > key)
                {
                    Swap(ref array[j - 1], ref array[j]);
                    j--;
                }

                array[j] = key;
            }

            return array;
        }
        
        //Сортировка Шелла
        public static SeparatedLine[] ShellSort(SeparatedLine[] input)
        {
            var array = new SeparatedLine[input.Length];
            Array.Copy(input, array, input.Length);
            //расстояние между элементами, которые сравниваются
            var d = array.Length / 2;
            while (d >= 1)
            {
                for (var i = d; i < array.Length; i++)
                {
                    var j = i;
                    while (j >= d && array[j - d] > array[j])
                    {
                        Swap(ref array[j], ref array[j - d]);
                        j -= d;
                    }
                }

                d /= 2;
            }

            return array;
        }


        public static SeparatedLine[] OrderBy(SeparatedLine[] input)
        {
            var array = new SeparatedLine[input.Length];
            Array.Copy(input, array, input.Length);
            return array.OrderBy(x => x).ToArray();
        }
        
        
        public static SeparatedLine[] Sort(SeparatedLine[] input)
        {
            var array = new SeparatedLine[input.Length];
            Array.Copy(input, array, input.Length);
            Array.Sort(array);
            return array;
        }
        
        
        //метод возвращающий индекс опорного элемента
        private static int Partition(SeparatedLine[] array, int minIndex, int maxIndex)
        {
            var pivot = minIndex - 1;
            for (var i = minIndex; i < maxIndex; i++)
            {
                if (array[i] < array[maxIndex])
                {
                    pivot++;
                    Swap(ref array[pivot], ref array[i]);
                }
            }

            pivot++;
            Swap(ref array[pivot], ref array[maxIndex]);
            return pivot;
        }

        //быстрая сортировка
        private static SeparatedLine[] QuickSort(SeparatedLine[] array, int minIndex, int maxIndex)
        {
            if (minIndex >= maxIndex)
                return array;

            var pivotIndex = Partition(array, minIndex, maxIndex);
            QuickSort(array, minIndex, pivotIndex - 1);
            QuickSort(array, pivotIndex + 1, maxIndex);
            return array;
        }
        
        static void QuickSortIterative(SeparatedLine[] arr, 
            int l, int h) 
        { 
            // Create an auxiliary stack 
            int[] stack = new int[h - l + 1]; 
  
            // initialize top of stack 
            int top = -1; 
  
            // push initial values of l and h to 
            // stack 
            stack[++top] = l; 
            stack[++top] = h; 
  
            // Keep popping from stack while 
            // is not empty 
            while (top >= 0) { 
                // Pop h and l 
                h = stack[top--]; 
                l = stack[top--]; 
  
                // Set pivot element at its 
                // correct position in 
                // sorted array 
                int p = Partition(arr, l, h); 
  
                // If there are elements on 
                // left side of pivot, then 
                // push left side to stack 
                if (p - 1 > l) { 
                    stack[++top] = l; 
                    stack[++top] = p - 1; 
                } 
  
                // If there are elements on 
                // right side of pivot, then 
                // push right side to stack 
                if (p + 1 < h) { 
                    stack[++top] = p + 1; 
                    stack[++top] = h; 
                } 
            } 
        }

        //TODO: Windows && Linux Winner
        public static SeparatedLine[] QuickSort(SeparatedLine[] input)
        {
            var array = new SeparatedLine[input.Length];
            Array.Copy(input, array, input.Length);
            QuickSortIterative(array, 0, input.Length - 1);
            return array;
        }
    }
}