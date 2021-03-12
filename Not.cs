using System;

namespace Not
{
    public static class Notation
    {
        public static Exception OutOfBaunds = new Exception("Out of baunds"); // выход за пределы введенной с.с.
        public static Exception OutOfNotation = new Exception("Out of Notation"); //выход зв пределы возможной системы счисления
        public static Exception StringIsEmpty = new Exception("string is empty"); //строка пуста
        public static Exception InCorrectSymbol = new Exception("incorrect symbol"); //введен некоректный символ

        private static int CharToInt(char sign) // перевод из char в int
        {
            if (sign >= '0' && sign <= '9') { return sign - '0'; }
            if (sign >= 'a' && sign <= 'z') { return sign - 'a' + 10; }
            if (sign >= 'A' && sign <= 'Z') { return sign - 'A' + 10; }
            throw InCorrectSymbol;
        }

        private static char IntToChar(int num)// перевод из int в char
        {
            if (num >= 0 && num <= 9) return (char)(num + '0');
            if (num >= 10 && num <= 36) return (char)(num + 'A' - 10);
            throw InCorrectSymbol;
        }

        private static double Notation_to10(int notation, string number) //перевод из одно с.с. в другую
        {
            if (number == "") //если строка пустая
            {
                throw StringIsEmpty;
            }
            if (notation > 36 || notation < 2)  //если выходит за возможные пределы
            {
                throw OutOfBaunds;
            }
            int n = number.Length; //длина введенной строки
            int dote_position = n; // позиции точки изначально задаем длину строки
            for (int i = 0; i < n; i++) // проходим по строке
            {
                if (number[i] == '.' || number[i] == ',') //ищем точку или запятую
                {
                    dote_position = i; //запоминаем позицию точки
                }
                else if (CharToInt(number[i]) >= notation) //проверяем на выход за пределы с.с.
                {
                    throw OutOfNotation;
                }
            }
            int whole_notationOf10 = 0; //целая часть числа в 10 с.с.
            double fractional_notationOf10 = 0; //дробная часть числа в 10 с.с.
            for (int i = 0; i < dote_position; i++) //проходим по целой части
            {
                whole_notationOf10 = whole_notationOf10 * notation + CharToInt(number[i]); //переводим целую часть в 10  с.с.
            }
            for (int i = n - 1; i > dote_position; i--)//проходим по дробной части
            {
                fractional_notationOf10 = (CharToInt(number[i]) + fractional_notationOf10) / notation; //переводим дробную часть в 10  с.с.
            }
            return Convert.ToDouble(whole_notationOf10 + fractional_notationOf10); //возвращаем число типа double 
        }

        private static string Notationfrom10ToAny(int notation, double number) //перевод из 10 с.с. в любую другую
        {
            if (notation == 10) //если введенная с.с. равна 10 
            {
                return number.ToString(); // возвращаем без изменений
            }
            string temp = number.ToString();
            int  n = temp.Length; //длина заданного числа
            int dote_position = n; // позиции точки изначально задаем длину строки
            string wholePart = ""; //целая часть
            for (int i = 0; i < dote_position; i++)  //идем до точки
            {
                if (temp[i] == '.' || temp[i] == ',') //ищем позицию точки
                {
                    dote_position = i;
                    break;
                }
                wholePart += temp[i]; // запиисываем целую часть
            }
            int whole = Convert.ToInt32(wholePart); //целая часть в формате int 
            wholePart = ""; //чистим целую часть для дальнейшего перевода в новую с.с.
            char c;
            while (whole != 0) //перевод в нужную нам с.с.
            {
                c = IntToChar(whole % notation);
                wholePart += c;
                whole /= notation;
            }
            int start = 0, end = wholePart.Length - 1;          //полученную строку  переворичваем в обратном порядке
            char[] s = wholePart.ToCharArray(); //разложим на массив char
            char temp_c;
            while (start <= end)
            {
                temp_c = s[start];
                s[start] = s[end];
                s[end] = temp_c;
                start++;
                end--;
            }
            wholePart = new string(s) ;     //массив char преобразуем обратно в string
            string fractionalPart = "0,"; // дробная часть
            for (int i = dote_position + 1; i < n; i++) //записываем дробную часть
            {
                fractionalPart += temp[i];
            }
            double fract = Convert.ToDouble(fractionalPart); //переменная для перевод в нужную нам с.с.
            fractionalPart = ",";
            if (wholePart == "") // если целая часть отсутствует ее значение равно 0
            {
                wholePart = "0";
            }
            int count = 0; //счетчик для огранечения чисел после запятой
            int a;
            while (fract != 0 && count != 10) //пока не станет равной 0 или не выйдет за пределы счетчика
            {
                a = (int)(fract * notation); 
                c = IntToChar(a);                                               // преобразование дробной части
                fractionalPart += c;                                            // в нужную с.с.
                fract = fract * notation - a;
                count++;
            }
            if (fractionalPart == ",") //если дробная часть пустая
            {
                return wholePart; //возвращаем целую
            }
            else 
            {
                return wholePart + fractionalPart; //иначе складываем обе части
            }
        }

        public static string NotationToAny(int from, int to, string number) //перевод из одной с.с. в другую
        {
            if (number == "") // пустая ли строка 
            {
                throw StringIsEmpty;
            }
            if ((from < 2 || from > 36) || (to < 2 || to > 36)) //не вышли ли за пределы
            {
                throw OutOfNotation;
            }
            int n = number.Length; 
            for (int i = 0; i < n; i++) //проверка на корректность числа
            {
                if (number[i] != '.' && number[i] != ',')
                {
                    if (CharToInt(number[i]) >= from)
                    {
                        throw OutOfBaunds;
                    }
                }
            }
            if (from == to) return number; //если с.с. одинаковые вернуть без имзменений
            else return Notationfrom10ToAny(to, Notation_to10(from, number)); 
           
        }
    }
}
