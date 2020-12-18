using UnityEngine;

namespace Assets.Scripts.Environment
{
    public class DisplayMenu : MonoBehaviour, IInteractableObject
    {
        public Menu menu;
        public void Interact()
        {
            menu.Display();
        }
    }
}