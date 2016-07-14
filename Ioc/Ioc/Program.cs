using System;
using System.Collections.Generic;
using System.Linq;

namespace Ioc
{
    class Program
    {
        static void Main(string[] args)
        {
            //ICreditCard creditCard = new MasterCard();
            //ICreditCard otherCreditCard = new Visa();

            Resolver resolver = new Resolver();
            //var shopper = new Shopper(resolver.ResolveCreditCard());
            //var shopper = new Shopper(otherCreditCard);

            resolver.Register<Shopper, Shopper>();
            resolver.Register<ICreditCard, MasterCard>();
            //resolver.Register<ICreditCard, Visa>();

            var shopper = resolver.Resolve<Shopper>();
            shopper.Charge();
            Console.ReadKey();
        }


        public class Resolver
        {
            //public ICreditCard ResolveCreditCard()
            //{
            //    if (new Random().Next(2) == 1)
            //        return new Visa();

            //    return new MasterCard();
            //}
            public Dictionary<Type, Type> dependencyMap = new Dictionary<Type, Type>();
            public T Resolve<T>()
            {
                return (T)Resolve(typeof (T));
            }

            public object Resolve(Type typeToResolve)
            {
                Type resolvedType = null;
                try
                {
                    resolvedType = dependencyMap[typeToResolve];
                }
                catch (Exception)
                {
                    throw new Exception(string.Format("Could not resolve type {0}", typeToResolve.FullName));
                }
                var firstConstructor = resolvedType.GetConstructors().First();
                var constructorParameter = firstConstructor.GetParameters();
                if (!constructorParameter.Any())
                    return Activator.CreateInstance(resolvedType);
                IList<object> parameters = new List<object>();
                foreach (var parameterToResolve in constructorParameter)
                {
                    parameters.Add(Resolve(parameterToResolve.ParameterType));
                }

                return firstConstructor.Invoke(parameters.ToArray());
            }

            public void Register<Tfrom, Tto>()
            {
                dependencyMap.Add(typeof(Tfrom), typeof(Tto));
            }
        }
        public class Shopper
        {
            private readonly ICreditCard _creditCard;

            public Shopper()
            {
                
            }
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