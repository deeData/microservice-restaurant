using System;

namespace PaymentProcessor
{
    public class ProcessPayment : IProcessPayment
    {
        public bool PaymentProcessor()
        {
            //implement custom logic and get card details etc
            
            //assumes payment went through successfully
            return true;
        }
    }
}
