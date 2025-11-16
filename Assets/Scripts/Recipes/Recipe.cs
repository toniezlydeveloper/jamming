using System;
using System.Collections.Generic;

namespace Core
{
    public class Recipe
    {
        public Func<List<Enum>, bool> IngredientsMatchingCallback { get; set; }
        public List<Enum> RequiredIngredients { get; set; }
        public List<float> NormalizedChances { get; set; }
        public ProcessorType ProcessorType { get; set; }
        public IngredientType OutputType { get; set; }
        public Enum Output { get; set; }
    }
}