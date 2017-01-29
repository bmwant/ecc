using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECC.EllipticCurveCryptography
{
    public class OperationsCounter
    {
        private int elementsAdd; // додавання/віднімання елементів у скінченному полі;
        private int elementsMultiply; // множення елементів у скінченному полі;
        private int elementsInverse; // пошук мультиплікативно оберненого елемента у скінченному полі;
        private int pointsAdd; // додавання точок еліптичної кривої;
        private int pointsDoubling; // подвоєння точки еліптичної кривої.
        private int totalOperations;  // Приблизна загальна кількість операцій для відслідковування прогресу
        private IProgress<int> progress;  // Репортування поточного прогресу для відображення змін в інтерфейсі
        public String Name;
        public OperationsCounter(int totalOperations = 0, IProgress<int> progress = null, String name = "-")
        {
            elementsAdd = 0;
            elementsMultiply = 0;
            elementsInverse = 0;
            pointsAdd = 0;
            pointsDoubling = 0;
            this.totalOperations = totalOperations;
            Name = name;
            if (progress != null) this.progress = progress;
        }

        public int Operations
        {
            get { return elementsAdd + elementsMultiply + elementsInverse + pointsAdd + pointsDoubling; }
        }

        public int AddPointsOperations
        {
            get { return pointsAdd; }
        }

        public int DoublingPointsOperations
        {
            get { return pointsDoubling;  }
        }

        public int Progress
        {
            get
            {
                if (this.totalOperations == 0) return 50; // ми не знаємо загальної кількості, тому повертаємо проміжне значення
                return (int)((double)this.Operations / this.totalOperations * 100);
            }
        }

        public void reportProgress()
        {
            if (progress != null)
            {
                Console.Write("Reporting progress" + this.Progress);
                progress.Report(this.Progress);
            }
        }

        public void opElementsAdd(int amount=1)
        {
            elementsAdd += amount;
            reportProgress();
        }

        public void opElementsMultiply(int amount=1)
        {
            elementsMultiply += amount;
            reportProgress();
        }

        public void opElementsInverse(int amount=1)
        {
            elementsInverse += amount;
            reportProgress();
        }
        public void opPointsAdd(int amount=1)
        {
            pointsAdd += amount;
            reportProgress();
        }

        public void opPointsDoubling(int amount=1)
        {
            pointsDoubling += amount;
            reportProgress();
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
