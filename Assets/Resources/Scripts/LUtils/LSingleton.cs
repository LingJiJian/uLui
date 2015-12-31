using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSingleton : MonoBehaviour
{
    private static GameObject m_Container = null;
    private static string m_Name = "Singleton";
    private static Dictionary<string, object> m_SingletonMap = new Dictionary<string, object>();
    private static bool m_IsDestroying = false;

    public static bool IsDestroying
    {
        get { return m_IsDestroying; }
    }

    public static bool IsCreatedInstance(string Name)
    {
        if (m_Container == null)
        {
            return false;
        }
        if (m_SingletonMap != null && m_SingletonMap.ContainsKey(Name))
        {
            return true;
        }
        return false;

    }
    public static object getInstance(string Name)
    {
        if (m_Container == null)
        {
            //Debug.Log("Create Singleton.");
            m_Container = new GameObject();
            m_Container.name = m_Name;
            m_Container.AddComponent(typeof(LSingleton));
        }
        if (!m_SingletonMap.ContainsKey(Name))
        {
            if (System.Type.GetType(Name) != null)
            {
                m_SingletonMap.Add(Name, m_Container.AddComponent(System.Type.GetType(Name)));
            }
            else
            {
                Debug.LogWarning("Singleton Type ERROR! (" + Name + ")");
            }
        }
        return m_SingletonMap[Name];
    }

    public static void RemoveInstance(string Name)
    {
        if (m_Container != null && m_SingletonMap.ContainsKey(Name))
        {
            UnityEngine.Object.Destroy((UnityEngine.Object)(m_SingletonMap[Name]));
            m_SingletonMap.Remove(Name);

            Debug.LogWarning("Singleton REMOVE! (" + Name + ")");
        }
    }

    void OnDestroy()
    {
        if (m_Container != null)
        {
            GameObject.Destroy(m_Container);
            m_Container = null;
            m_IsDestroying = true;
            m_SingletonMap.Clear();
        }
    }

    void Awake()
    {
        //DontDestroyOnLoad(gameObject);
    }

    void OnApplicationQuit()
    {
        if (m_Container != null)
        {
            GameObject.Destroy(m_Container);
            m_Container = null;
            m_IsDestroying = true;
        }
    }

}