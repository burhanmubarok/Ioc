﻿using System;

namespace Ioc
{
    class Program
    {
        static void Main(string[] args)
        {
            ICreditCard creditCard = new MasterCard();
            ICreditCard otherCreditCard = new Visa();
            var shopper = new Shopper(otherCreditCard);
            shopper.Charge();
            Console.ReadKey();
        }

        public class Shopper
        {
            private readonly ICreditCard _creditCard;

            public Shopper(ICreditCard creditCard)
            {
                _creditCard = creditCard;
            }

            public void Charge()
            {
                var chargeMessage =_creditCard.Charge();
                Console.WriteLine(chargeMessage);
            }
        }
    }

    internal class Visa : ICreditCard
    {
        public string Charge()
        {
            return "Charging with the visa";
        }
    }

    internal class MasterCard : ICreditCard
    {
        public string Charge()
        {
            return "Swiping the mastercard";
        }
    }

    internal interface ICreditCard
    {
        string Charge();
    }
}