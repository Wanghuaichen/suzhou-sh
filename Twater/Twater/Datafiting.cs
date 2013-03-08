using System;
using System.Collections.Generic;
using System.Text;

using System.Globalization;

namespace Twater
{
    public static class Datafiting
    {
        ///<summary>
        ///����С���˷���϶�Ԫ�������
        ///</summary>
        ///<param name="arrX">��֪���x���꼯��</param>
        ///<param name="arrY">��֪���y���꼯��</param>
        ///<param name="length">��֪��ĸ���</param>
        ///<param name="dimension">���̵���ߴ���</param>

        public static double[] MultiLine(double[] arrX, double[] arrY, int length, int dimension)//��Ԫ������Է����������
        {
            int n = dimension + 1; //dimension�η�����Ҫ�� dimension+1�� ϵ��
            double[,] Guass = new double[n, n + 1]; //��˹���� ���磺y=a0+a1*x+a2*x*x
            for (int i = 0; i < n; i++)
            {
                int j;
                for (j = 0; j < n; j++)
                {
                    Guass[i, j] = SumArr(arrX, j + i, length);
                }
                Guass[i, j] = SumArr(arrX, i, arrY, 1, length);
            }
            return ComputGauss(Guass, n);
        }
        /// <summary>
        /// �������Ԫ�ص�n�η��ĺ�
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="n"></param>
        /// <param name="length">��֪��ĸ���</param>
        /// <returns></returns>
        public static double SumArr(double[] arr, int n, int length) //�������Ԫ�ص�n�η��ĺ�
        {
            double s = 0;
            for (int i = 0; i < length; i++)
            {
                if (arr[i] != 0 || n != 0)
                    s = s + Math.Pow(arr[i], n);
                else
                    s = s + 1;
            }
            return s;
        }
        public static double SumArr(double[] arr1, int n1, double[] arr2, int n2, int length)
        {
            double s = 0;
            for (int i = 0; i < length; i++)
            {
                if ((arr1[i] != 0 || n1 != 0) && (arr2[i] != 0 || n2 != 0))
                    s = s + Math.Pow(arr1[i], n1) * Math.Pow(arr2[i], n2);
                else
                    s = s + 1;
            }
            return s;

        }
        public static double[] ComputGauss(double[,] Guass, int n)
        {
            int i, j;
            int k, m;
            double temp;
            double max;
            double s;
            double[] x = new double[n];
            for (i = 0; i < n; i++) x[i] = 0.0;//��ʼ��

            for (j = 0; j < n; j++)
            {
                max = 0;
                k = j;
                for (i = j; i < n; i++)
                {
                    if (Math.Abs(Guass[i, j]) > max)
                    {
                        max = Guass[i, j];
                        k = i;
                    }
                }


                if (k != j)
                {
                    for (m = j; m < n + 1; m++)
                    {
                        temp = Guass[j, m];
                        Guass[j, m] = Guass[k, m];
                        Guass[k, m] = temp;
                    }
                }
                if (0 == max)
                {
                    // "�����Է���Ϊ�������Է���" 
                    return x;
                }

                for (i = j + 1; i < n; i++)
                {
                    s = Guass[i, j];
                    for (m = j; m < n + 1; m++)
                    {
                        Guass[i, m] = Guass[i, m] - Guass[j, m] * s / (Guass[j, j]);
                    }
                }

            }//����for (j=0;j<n;j++)

            for (i = n - 1; i >= 0; i--)
            {
                s = 0;
                for (j = i + 1; j < n; j++)
                {
                    s = s + Guass[i, j] * x[j];
                }
                x[i] = (Guass[i, n] - s) / Guass[i, i];
            }
            return x;
        }//����ֵ�Ǻ�����ϵ��
    }
}
