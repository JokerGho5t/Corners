using UnityEngine;

    public enum TypeMessage
    {
        Message,
        Warning,
        Error
    }

public class Log
{
            public static void LogMe(string ClassName, string message, TypeMessage type = TypeMessage.Message) //Дебаг листа уровня
        {
            switch (type)
            {
                case TypeMessage.Message:
                    Debug.Log("Class: " + ClassName + "\n" + "Message: " + message);
                    break;
                case TypeMessage.Warning:
                    Debug.LogWarning("Class: " + ClassName + "\n" + "Message: " + message);
                    break;
                case TypeMessage.Error:
                    Debug.LogError("Class: " + ClassName + "\n" + "Message: " + message);
                    break;
            }
        }
}
