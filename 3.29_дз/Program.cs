using _3._29_дз;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*Разработать приложение «Резервная копия»
Цель: произвести расчет необходимого количества внешних носителей информации при переносе за один раз важной информации (565 Гб, файлы по 780 Мб) с рабочего компьютера на домашний компьютер и затрачиваемое на данный процесс время. Вы имеете в распоряжении следующие типы носителей информации:
■ Flash - память,
■ DVD - диск,
■ съемный HDD.
Каждый носитель информации является объектом соответствующего класса*/

namespace _3._29_дз
{
    abstract class Storage
    {
        protected string name { get; set; }
        public string model { get; set; }
        protected double speed {  get; set; }

        protected double whole_memory {  get; set; }
        protected double taken_memory {  get; set; }

        public Storage(string name, string model, double speed)
        {
            this.name = name;
            this.model = model;
            this.speed = speed;
        }
        public double getMemory()
        {
            return whole_memory;
        }
        public virtual void Copy(ref double data, ref double time)
        {
            if (data > whole_memory)
            {
                taken_memory = whole_memory;
                data -= whole_memory;
                time += data / speed;
            }
            else
            {
                taken_memory = data;
                time += data / speed;
                data = 0;
            }
            
        }
        public virtual void getInfo()
        {
            Console.WriteLine($"Имя - {name}, модель - {model}, скорость - {speed} гб, объем - {whole_memory} гб ,занято памяти - {taken_memory} гб");
        }
        public double getFreeSpace() { return whole_memory - taken_memory; }

    }
    class Flash : Storage
    {
        public Flash(string name, string model, double memory): base(name, model, 5)
        {
            whole_memory = memory;
            taken_memory = 0;
        }
    }

    class DVD : Storage
    {
        int type;
        public DVD(string name, string model, double speed, int type) : base(name, model, speed)
        {
            this.type = type;
            if (type == 1) whole_memory = 4.7;
            else if (type == 2) whole_memory = 9;
            taken_memory = 0;
        }

        public override void getInfo()
        {
            Console.WriteLine($"Имя - {name}, модель - {model}, скорость - {speed} гб, тип - {(type==1?"одностороний":"двусторонний")}, объем - {whole_memory} гб, занято памяти - {taken_memory} гб");
        }
    }

    class HDD : Storage
    {
        int counter;
        public HDD(string name, string model, int counter, int memory) : base(name, model, 0.47)
        {
            taken_memory = 0;
            this.counter = counter;
            whole_memory = memory * counter;
        }
        public override void getInfo()
        {
            Console.WriteLine($"Имя - {name}, модель - {model}, скорость - {speed} гб, количество разделов - {counter}, объем - {whole_memory} гб, занято памяти - {taken_memory} гб");
        }
    }
    internal class Program
    {
        static void add(ref Storage[] arr)
        {
            if (arr[0] is Flash flash) {
                arr = new List<Storage>(arr) { new Flash("New flashdrie", "flash", 16) }.ToArray();
            }
            else if (arr[0] is DVD dvd)
            {
                arr = new List<Storage>(arr) { new DVD("New disk", "dvd", 0.4, 2) }.ToArray();
            }
            else if (arr[0] is HDD hdd)
            {
                arr = new List<Storage>(arr) { new HDD("New hdd", "hdd", 2, 512) }.ToArray();
            }
        }
        static void Main(string[] args)
        {

            Storage[] storage = { new DVD("C", "dvd", 100, 2) };
            double data = 584;
            double time = 0;

            while (data != 0)
            {
                foreach (Storage s in storage)
                {
                    if (s.getFreeSpace() != 0 && data != 0)
                    {
                        s.Copy(ref data, ref time);
                    }
                    else
                    {
                        continue;
                    }
                }

                if (data != 0)
                { 
                    add(ref storage);
                }
            }

            Console.WriteLine($"Расчетное время - {time / 60} минут. Понадобится {storage.Length} внешних носителей информации");

            Console.ReadKey();

        }
    }
}
