using System;

namespace FanOutPutLib
{
    public class FanOutPut
    {
        private string _name;
        private int _temp;
        private int _moisture;
        public int Id { get; set; }

        public string Name
        {
            get { return _name; }
            set
            {
                if (value.Length < 2)
                {
                    throw new ArgumentException("Name length can't less than two characters");
                }
                else
                {
                    _name = value;
                }
            }
        }

        public int Temp
        {
            get { return _temp; }
            set
            {
                if (value >= 15 && value <= 25)
                {
                    _temp = value;
                }
                else
                {
                    throw new ArgumentException("The temperature cant be less than 15 or greater than 25");
                }

            }
        }

        public int Moisture
        {
            get { return _moisture; }
            set
            {
                if (value >= 30 && value <= 80)
                {
                    _moisture = value;
                }
                else
                {
                    throw new ArgumentException("Moisture not correct");
                }

            }
        }

        public FanOutPut()
        {

        }

        public FanOutPut(int id, string name, int temp, int moisture)
        {
            Id = id;
            Name = name;
            Temp = temp;
            Moisture = moisture;
        }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Name)}: {Name}, {nameof(Temp)}: {Temp}, {nameof(Moisture)}: {Moisture}";
        }
    }
}
