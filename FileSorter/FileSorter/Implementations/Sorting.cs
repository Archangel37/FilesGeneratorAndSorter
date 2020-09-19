using System;
using System.Collections.Generic;
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

            for (var i = 0; i < array.Length - 1; i++)
            for (var j = i + 1; j > 0; j--)
                if (array[j - 1] > array[j])
                {
                    var temp = array[j - 1];
                    array[j - 1] = array[j];
                    array[j] = temp;
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
                if (array[i] < array[maxIndex])
                {
                    pivot++;
                    Swap(ref array[pivot], ref array[i]);
                }

            pivot++;
            Swap(ref array[pivot], ref array[maxIndex]);
            return pivot;
        }

        //быстрая сортировка с рекурсией
        // private static SeparatedLine[] QuickSort(SeparatedLine[] array, int minIndex, int maxIndex)
        // {
        //     if (minIndex >= maxIndex)
        //         return array;
        //
        //     var pivotIndex = Partition(array, minIndex, maxIndex);
        //     QuickSort(array, minIndex, pivotIndex - 1);
        //     QuickSort(array, pivotIndex + 1, maxIndex);
        //     return array;
        // }

        private static void QuickSortIterative(SeparatedLine[] arr,
            int l, int h)
        {
            // Create an auxiliary stack 
            var stack = new int[h - l + 1];

            // initialize top of stack 
            var top = -1;

            // push initial values of l and h to 
            // stack 
            stack[++top] = l;
            stack[++top] = h;

            // Keep popping from stack while 
            // is not empty 
            while (top >= 0)
            {
                // Pop h and l 
                h = stack[top--];
                l = stack[top--];

                // Set pivot element at its 
                // correct position in 
                // sorted array 
                var p = Partition(arr, l, h);

                // If there are elements on 
                // left side of pivot, then 
                // push left side to stack 
                if (p - 1 > l)
                {
                    stack[++top] = l;
                    stack[++top] = p - 1;
                }

                // If there are elements on 
                // right side of pivot, then 
                // push right side to stack 
                if (p + 1 < h)
                {
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
        
        
        public static SeparatedLine[] HybridOptimizedQuickSort(SeparatedLine[] input)
        {
            var array = new SeparatedLine[input.Length];
            Array.Copy(input, array, input.Length);
            OptimizedQuickSort(array, 0, input.Length - 1);
            return array;
        }
        
        
        //todo
        private static void OptimizedQuickSort(SeparatedLine[] arr, int low, int high)
        {
            while (low < high)
            {
                // do insertion sort if 10 or smaller
                if (high - low < 10)
                {
                    InsertionSortForOptimization(arr, low, high);
                    break;
                }
                var pivot = Partition(arr, low, high);
                // tail call optimizations - recur on smaller sub-array
                if (pivot - low < high - pivot) {
                    OptimizedQuickSort(arr, low, pivot - 1);
                    low = pivot + 1;
                } else {
                    OptimizedQuickSort(arr, pivot + 1, high);
                    high = pivot - 1;
                }
            }
        }
        
        private static void InsertionSortForOptimization(IList<SeparatedLine> arr, int low, int high)
        {
            // Start from second element (element at index 0
            // is already sorted)
            for (var i = low + 1; i <= high; i++)
            {
                var value = arr[i];
                var j = i;

                // Find the index j within the sorted subset arr[0..i-1]
                // where element arr[i] belongs
                while (j > low && arr[j - 1] > value)
                {
                    arr[j] = arr[j - 1];
                    j--;
                }
                // Note that subarray arr[j..i-1] is shifted to
                // the right by one position i.e. arr[j+1..i]
                arr[j] = value;
            }
        }

        public static SeparatedLine[] HeapSorting(SeparatedLine[] input)
        {
            var array = new SeparatedLine[input.Length];
            Array.Copy(input, array, input.Length);
            HeapSort(array, input.Length);
            return array;
        }

        // To heapify a subtree rooted with node i which is 
        // an index in arr[]. n is size of heap 
        private static void Heapify(SeparatedLine[] arr, int n, int i)
        {
            var largest = i; // Initialize largest as root 
            var l = 2 * i + 1; // left = 2*i + 1 
            var r = 2 * i + 2; // right = 2*i + 2 

            // If left child is larger than root 
            if (l < n && arr[l] > arr[largest])
                largest = l;

            // If right child is larger than largest so far 
            if (r < n && arr[r] > arr[largest])
                largest = r;

            // If largest is not root 
            if (largest != i)
            {
                Swap(ref arr[i], ref arr[largest]);

                // Recursively heapify the affected sub-tree 
                Heapify(arr, n, largest);
            }
        }

        // main function to do heap sort 
        static void HeapSort(SeparatedLine[] arr, int n)
        {
            // Build heap (rearrange array) 
            for (var i = n / 2 - 1; i >= 0; i--)
                Heapify(arr, n, i);

            // One by one extract an element from heap 
            for (var i = n - 1; i > 0; i--)
            {
                // Move current root to end 
                Swap(ref arr[0], ref arr[i]);

                // call max heapify on the reduced heap 
                Heapify(arr, i, 0);
            }
        }
    }
}