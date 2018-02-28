using StardewValley;
using StardewValley.TerrainFeatures;
using SObject = StardewValley.Object;

namespace Pathoschild.Stardew.Automate.Framework.Machines.TerrainFeatures
{
    /// <summary>A fruit tree machine that accepts input and provides output.</summary>
    /// <remarks>Derived from <see cref="FruitTree.shake"/>.</remarks>
    internal class FlooringMachine : IMachine
    {
        /// <summary>Get the machine's processing state.</summary>
        public MachineState GetState()
        {
            return MachineState.Processing;
        }

        /// <summary>Get the output item.</summary>
        public ITrackedStack GetOutput()
        {
            return null;
        }

        /// <summary>Provide input to the machine.</summary>
        /// <param name="input">The available items.</param>
        /// <returns>Returns whether the machine started processing an item.</returns>
        public bool SetInput(IStorage input)
        {
            return false; // no input
        }
    }
}
