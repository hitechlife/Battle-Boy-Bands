     using UnityEngine;
     using UnityEngine.EventSystems;
     
    // https://answers.unity.com/questions/1362254/how-to-keep-a-ui-button-stay-highlighted-or-at-lea.html
     public class EventSystemKeepSelected : MonoBehaviour
     {
         private EventSystem eventSystem;
         private GameObject lastSelected = null;
         void Start()
         {
             eventSystem = GetComponent<EventSystem>();
         }
     
        private bool updatedChoice = false;
         void Update()
         {
             if (eventSystem != null)
             {
                 if (BeatManager.S.isPlayerLoop && !updatedChoice) {
                     lastSelected = null;
                     updatedChoice = true;
                 } else if (updatedChoice && BeatManager.S.isPlayerLoop == false) {
                     updatedChoice = false;
                 }

                 if (eventSystem.currentSelectedGameObject != null)
                 {
                     lastSelected = eventSystem.currentSelectedGameObject;
                 }
                 else if (lastSelected != null)
                 {
                     eventSystem.SetSelectedGameObject(lastSelected);
                 }
             }
         }
     }