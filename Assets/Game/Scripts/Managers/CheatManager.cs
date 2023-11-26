using UnityEngine;
using Zenject;

public class CheatManager : MonoBehaviour {

    [Inject]
    private void Construct() {
        //Прокидывать зависимости на модули в которых нужно задействовать читы.
    }

    private void Update() {
        //Проверять нажатие клавиши и запускать нужную чит команду в сервисе.
    }
}
