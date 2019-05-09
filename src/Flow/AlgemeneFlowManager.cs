using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Bubble {

    abstract class HardFlowClass {
        abstract public void Draw();
        abstract public void Update();
    }



    static class FlowManager {

        static Dictionary<string, HardFlowClass> HardFlow = new Dictionary<string, HardFlowClass>();

    }
}
