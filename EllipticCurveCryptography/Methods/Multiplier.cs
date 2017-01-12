using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Methods
{
    public delegate void OperationEventHandler(double time);

    abstract class Multiplier
    {
        public event OperationEventHandler Multiplication;
        public event OperationEventHandler Addition;
        public event OperationEventHandler Double;
        public event OperationEventHandler MultInverse;

        //public abstract void 

        protected virtual void OnMultiplied(double e)
        {
            if (Multiplication != null)
                Multiplication(e);
        }
        protected virtual void OnAdded(double e)
        {
            if (Addition != null)
                Addition(e);
        }
        protected virtual void OnDoubled(double e)
        {
            if (Double != null)
                Double(e);
        }
        protected virtual void OnMultInverse(double e)
        {
            if (MultInverse != null)
                MultInverse(e);
        }

    }
}
