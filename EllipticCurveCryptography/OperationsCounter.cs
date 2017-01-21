using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECC.EllipticCurveCryptography
{
    class OperationsCounter
    {
        private int elementsAdd; // додавання/віднімання елементів у скінченному полі;
        private int elementsMultiply; // множення елементів у скінченному полі;
        private int elementsInverse; // пошук мультиплікативно оберненого елемента у скінченному полі;
        private int pointsAdd; // додавання точок еліптичної кривої;
        private int pointsDoubling; // подвоєння точки еліптичної кривої.
        public OperationsCounter()
        {
            elementsAdd = 0;
            elementsMultiply = 0;
            elementsInverse = 0;
            pointsAdd = 0;
            pointsDoubling = 0;
   
        }

        public void opElementsAdd()
        {
            elementsAdd++;
        }
        public void opElementsMultiply()
        {
            elementsMultiply++;
        }
        public void opElementsInverse()
        {
            elementsInverse++;
        }
        public void opPointsAdd()
        {
            pointsAdd++;
        }
        public void opPointsDoubling()
        {
            pointsDoubling++;
        }
        public override string ToString()
        {
            return "Загальна кількість операцій:\n" +
                "1) додавання/віднімання елементів у скінченному полі: " + elementsAdd + "\n" +
                "2) множення елементів у скінченному полі: " + elementsMultiply + "\n" +
                "3) пошук мультиплікативно оберненого елемента у скінченному полі: " + elementsInverse + "\n" +
                "4) додавання точок еліптичної кривої: " + pointsAdd + "\n" +
                "5) подвоєння точки еліптичної кривої." + pointsDoubling + "\n";
        }
    }
}
