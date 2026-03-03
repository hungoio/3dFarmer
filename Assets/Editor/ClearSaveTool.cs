using UnityEngine;
using UnityEditor;
using System.IO;

public class ClearSaveTool
{
    // Tạo một nút bấm trên thanh Menu của Unity
    [MenuItem("Tools/Xóa Dữ Liệu Save (Clear Save)")]
    public static void ClearData()
    {
        // 1. Xóa các dữ liệu lưu tạm (PlayerPrefs)
        PlayerPrefs.DeleteAll();

        // 2. Xóa các file Save vật lý ẩn trong máy tính
        string path = Application.persistentDataPath;
        DirectoryInfo directory = new DirectoryInfo(path);

        foreach (FileInfo file in directory.GetFiles())
        {
            file.Delete(); // Xóa từng file save cũ
        }

        // Báo cáo thành công xuống bảng Console
        Debug.Log("🟢 ĐÃ XÓA TOÀN BỘ FILE SAVE CŨ! Bạn có thể bật lại FarmSaveManager.");
    }
}