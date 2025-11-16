using Flow.States;
using Internal.Runtime.Flow.UI;
using UI.Panels;

namespace UI
{
    public class UIPanelsProvider : AUIPanelsProvider
    {
        protected override void AddTranslations()
        {
            AddTranslation<GameplayState, GameplayPanel>();
            AddTranslation<MenuState, MenuPanel>();
            AddTranslation<RecipesCheckState, RecipesPanel>();
        }
    }
}