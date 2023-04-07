using System.ComponentModel;
using System.IO;
using SRF.Service;
using UnityEngine;
using UnityEngine.Scripting;

public delegate void SROptionsPropertyChanged(object sender, string propertyName);

#if !DISABLE_SRDEBUGGER
[Preserve]
#endif
public partial class SROptions : INotifyPropertyChanged
{
    private static readonly SROptions _current = new SROptions();

    public static SROptions Current
    {
        get { return _current; }
    }

#if !DISABLE_SRDEBUGGER
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void OnStartup()
    {
        SRServiceManager.GetService<SRDebugger.Internal.InternalOptionsRegistry>().AddOptionContainer(Current);
    }
#endif

    public event SROptionsPropertyChanged PropertyChanged;
    
#if UNITY_EDITOR
    [JetBrains.Annotations.NotifyPropertyChangedInvocator]
#endif
    public void OnPropertyChanged(string propertyName)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, propertyName);
        }

        if (InterfacePropertyChangedEventHandler != null)
        {
            InterfacePropertyChangedEventHandler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    private event PropertyChangedEventHandler InterfacePropertyChangedEventHandler;

    event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
    {
        add { InterfacePropertyChangedEventHandler += value; }
        remove { InterfacePropertyChangedEventHandler -= value; }
    }
    public const string CAR_ID = "切换车辆ID";
    public const string URL_PATH = "测试环境:1线上,2测试";
    
    [Category("调试"),DisplayName(SROptions.CAR_ID),Sort(0)]
    public string carID{
        get
        {
            if(_carID == null)
            {
                _carID = PlayerPrefs.GetString(SROptions.CAR_ID);
            }
            return _carID;
        }
        set
        {
            _carID = value;
        }
    }
    private string _carID;

    [Category("调试"),DisplayName(URL_PATH),Sort(1)]
    public string isOnline{
        get
        {
            if(_isOnline == null)
            {
                _isOnline = PlayerPrefs.GetString(URL_PATH);
            }
            return _isOnline;
        }
        set
        {
            _isOnline = value;
        }
    }
    private string _isOnline;



    [Category("调试"),DisplayName("保存"),Sort(2)]
    public void SetCarID()
    {
        PlayerPrefs.SetString(CAR_ID,_carID);
        PlayerPrefs.SetString(URL_PATH,_isOnline);
        Debug.Log(IsOnline());
    }

    [Category("调试"),DisplayName("清理缓存"),Sort(3)]
    public void ClearCache()
    {
        string path =  Application.persistentDataPath;
        DirectoryInfo dir = new DirectoryInfo(path);
        FileSystemInfo[] fileSystems = dir.GetFileSystemInfos();
        foreach (var item in fileSystems)
        {
            if (item is DirectoryInfo)//判断是否是文件夹
            {
                DirectoryInfo directory = new DirectoryInfo(item.FullName);
                directory.Delete(true);
            }
            else
            {
                File.Delete(item.FullName);//删除这个文件
            }
        }
    }


    public static bool IsOnline()
    {
        if(string.IsNullOrEmpty(PlayerPrefs.GetString(URL_PATH)))
        {
            return false;
        }
        else if(PlayerPrefs.GetString(URL_PATH) == "1")
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
