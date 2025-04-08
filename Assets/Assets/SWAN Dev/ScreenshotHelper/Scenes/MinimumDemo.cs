
using UnityEngine;

public class MinimumDemo : MonoBehaviour
{
    public Camera m_Camera;
    public MeshRenderer m_CubeMeshRenderer;

    [Space]
    public SDev.FileSaveUtil.AppPath ApplicationPath = SDev.FileSaveUtil.AppPath.PersistentDataPath;
    public string SubFolderName;
    public string FileName;

    private Texture2D _texture2D;

    public void TakeScreenshot()
    {
        //ScreenshotHelper.iClear(); // Clear the old texture (if any)
        ScreenshotHelper.iCaptureScreen((texture2D) =>
        {
            if (_texture2D) Destroy(_texture2D); // Manually clear the old texture
            _texture2D = texture2D;

            // Set the new (captured) screenshot texture to the cube renderer.
            m_CubeMeshRenderer.material.mainTexture = texture2D;

            SaveTexture(texture2D);
        });
    }

    public void CaptureWithCamera()
    {
        //ScreenshotHelper.iClear(); // Clear the old texture (if any)
        ScreenshotHelper.iCaptureWithCamera(m_Camera, (texture2D) =>
        {
            if (_texture2D) Destroy(_texture2D); // Manually clear the old texture
            _texture2D = texture2D;

            // Set the new (captured) screenshot texture to the cube renderer.
            m_CubeMeshRenderer.material.mainTexture = texture2D;

            SaveTexture(texture2D);
        });
    }

    private void SaveTexture(Texture2D texture2D)
    {
        // Example: Save to Application data path
        string savePath = SDev.FileSaveUtil.Instance.SaveTextureAsJPG(texture2D, ApplicationPath, SubFolderName, FileName);
        //string savePath = SDev.FileSaveUtil.Instance.SaveTextureAsJPG(texture2D, System.Environment.SpecialFolder.MyPictures, SubFolderName, FileName);
        Debug.Log("Result - Texture resolution: " + texture2D.width + " x " + texture2D.height + "\nSaved at: " + savePath);


        // ----- Below are other save methods for different Unity player platforms (ScreenshotHelper Plus or related plugin required) -----

        // Example: Save to mobile device gallery(iOS/Android). <- Requires Mobile Media Plugin (Included in Screenshot Helper Plus, and SwanDev GIF Assets)
        //MobileMedia.SaveImage(texture2D, SubFolderName, FileName, MobileMedia.ImageFormat.JPG);

        // Example: Save to persistent data path on WebGL
        //savePath = SDev.EasyIO.SaveImage(texture2D, SDev.EasyIO.ImageEncodeFormat.JPG, FileName + ".jpg", SubFolderName);

        // Example: Open the 'Save As' dialog box on WebGL
        //SDev.EasyIO.WebGL_SaveToLocal(texture2D.EncodeToJPG(), FileName + ".jpg");
    }

    public void Clear()
    {
        ScreenshotHelper.iClear();
    }

    public void OpenSaveFolder()
    {
        string dir = SDev.FileSaveUtil.Instance.GetAppPath(ApplicationPath);
        string path = System.IO.Path.Combine(dir, SubFolderName);
        if (!System.IO.Directory.Exists(path)) System.IO.Directory.CreateDirectory(path);

#if UNITY_EDITOR_OSX
        System.Diagnostics.Process.Start(path);
#else
        Application.OpenURL(path);
#endif
    }
}
