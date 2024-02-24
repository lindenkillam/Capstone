﻿#if UNITY_STANDALONE_LINUX || CT_DEVELOP
using System;
using UnityEngine;

namespace Crosstales.FB.Wrapper
{
   /// <summary>File browser implementation for Linux (GTK).</summary>
   public class FileBrowserLinux : BaseFileBrowser
   {
      #region Variables

      private static Action<string[]> _openFileCb;
      private static Action<string[]> _openFolderCb;
      private static Action<string> _saveFileCb;

      private const char splitChar = (char)28;

      #endregion


      #region Constructor

      public FileBrowserLinux()
      {
         Linux.NativeMethods.DialogInit();
      }

      #endregion


      #region Implemented methods

      public override bool canOpenFile => true;
      public override bool canOpenFolder => true;
      public override bool canSaveFile => true;

      public override bool canOpenMultipleFiles => true;

      public override bool canOpenMultipleFolders => true;

      public override bool isPlatformSupported => Util.Helper.isLinuxPlatform; // || Util.Helper.isLinuxEditor;

      public override bool isWorkingInEditor => false;

      public override string CurrentOpenSingleFile { get; set; }
      public override string[] CurrentOpenFiles { get; set; }
      public override string CurrentOpenSingleFolder { get; set; }
      public override string[] CurrentOpenFolders { get; set; }
      public override string CurrentSaveFile { get; set; }

      public override string[] OpenFiles(string title, string directory, string defaultName, bool multiselect, params ExtensionFilter[] extensions)
      {
         if (!string.IsNullOrEmpty(defaultName))
            Debug.LogWarning("'defaultName' is not supported under Linux.");

         //directory += System.IO.Path.DirectorySeparatorChar + defaultName; //TODO works?

         string paths = System.Runtime.InteropServices.Marshal.PtrToStringAnsi(Linux.NativeMethods.DialogOpenFilePanel(title, directory, getFilterFromFileExtensionList(extensions), multiselect));

         if (string.IsNullOrEmpty(paths))
            return null;

         string[] pathArray = paths.Split(splitChar);

         CurrentOpenFiles = pathArray;
         CurrentOpenSingleFile = pathArray[0];

         return CurrentOpenFiles;
      }

      public override string[] OpenFolders(string title, string directory, bool multiselect)
      {
         string paths = System.Runtime.InteropServices.Marshal.PtrToStringAnsi(Linux.NativeMethods.DialogOpenFolderPanel(title, directory, multiselect));

         if (string.IsNullOrEmpty(paths))
            return null;

         string[] pathArray = paths.Split(splitChar);

         CurrentOpenFolders = pathArray;
         CurrentOpenSingleFolder = pathArray[0];

         return CurrentOpenFolders;
      }

      public override string SaveFile(string title, string directory, string defaultName, params ExtensionFilter[] extensions)
      {
         string path = System.Runtime.InteropServices.Marshal.PtrToStringAnsi(Linux.NativeMethods.DialogSaveFilePanel(title, directory, defaultName, getFilterFromFileExtensionList(extensions)));

         if (string.IsNullOrEmpty(path))
            return null;

         CurrentSaveFile = path;

         return CurrentSaveFile;
      }

      public override void OpenFilesAsync(string title, string directory, string defaultName, bool multiselect, ExtensionFilter[] extensions, Action<string[]> cb)
      {
         if (!string.IsNullOrEmpty(defaultName))
            Debug.LogWarning("'defaultName' is not supported under Linux.");

         //directory += System.IO.Path.DirectorySeparatorChar + defaultName; //TODO works?

         _openFileCb = cb;
         Linux.NativeMethods.DialogOpenFilePanelAsync(
            title,
            directory,
            getFilterFromFileExtensionList(extensions),
            multiselect,
            paths =>
            {
               if (string.IsNullOrEmpty(paths))
               {
                  _openFileCb?.Invoke(null);
               }
               else
               {
                  string[] pathArray = paths.Split(splitChar);

                  CurrentOpenFiles = pathArray;
                  CurrentOpenSingleFile = pathArray[0];

                  _openFileCb?.Invoke(pathArray);
               }
            });
      }

      public override void OpenFoldersAsync(string title, string directory, bool multiselect, Action<string[]> cb)
      {
         _openFolderCb = cb;
         Linux.NativeMethods.DialogOpenFolderPanelAsync(
            title,
            directory,
            multiselect,
            paths =>
            {
               if (string.IsNullOrEmpty(paths))
               {
                  _openFolderCb?.Invoke(null);
               }
               else
               {
                  string[] pathArray = paths.Split(splitChar);

                  CurrentOpenFolders = pathArray;
                  CurrentOpenSingleFolder = pathArray[0];

                  _openFolderCb?.Invoke(pathArray);
               }
            });
      }

      public override void SaveFileAsync(string title, string directory, string defaultName, ExtensionFilter[] extensions, Action<string> cb)
      {
         _saveFileCb = cb;
         Linux.NativeMethods.DialogSaveFilePanelAsync(
            title,
            directory,
            defaultName,
            getFilterFromFileExtensionList(extensions),
            path =>
            {
               if (string.IsNullOrEmpty(path))
               {
                  _saveFileCb?.Invoke(null);
               }
               else
               {
                  CurrentSaveFile = path;

                  _saveFileCb?.Invoke(path);
               }
            });
      }

      #endregion


      #region Private methods

      private static string getFilterFromFileExtensionList(ExtensionFilter[] extensions)
      {
         if (extensions?.Length > 0)
         {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            for (int xx = 0; xx < extensions.Length; xx++)
            {
               ExtensionFilter filter = extensions[xx];

               sb.Append(filter.Name);
               sb.Append(";");

               for (int ii = 0; ii < filter.Extensions.Length; ii++)
               {
                  sb.Append(filter.Extensions[ii]);

                  if (ii + 1 < filter.Extensions.Length)
                     sb.Append(",");
               }

               if (xx + 1 < extensions.Length)
                  sb.Append("|");
            }

            if (Util.Config.DEBUG)
               Debug.Log($"getFilterFromFileExtensionList: {sb}");

            return sb.ToString();
         }

         return string.Empty;
      }

      #endregion
   }
}

namespace Crosstales.FB.Wrapper.Linux
{
   /// <summary>Native methods (bridge to Linux).</summary>
   internal static class NativeMethods
   {
      [System.Runtime.InteropServices.UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.StdCall)]
      public delegate void AsyncCallback(string path);

      [System.Runtime.InteropServices.DllImport("FileBrowser")]
      internal static extern void DialogInit();

      [System.Runtime.InteropServices.DllImport("FileBrowser")]
      internal static extern IntPtr DialogOpenFilePanel(string title, string directory, string extension, bool multiselect);

      [System.Runtime.InteropServices.DllImport("FileBrowser")]
      internal static extern IntPtr DialogOpenFolderPanel(string title, string directory, bool multiselect);

      [System.Runtime.InteropServices.DllImport("FileBrowser")]
      internal static extern IntPtr DialogSaveFilePanel(string title, string directory, string defaultName, string extension);

      [System.Runtime.InteropServices.DllImport("FileBrowser")]
      internal static extern void DialogOpenFilePanelAsync(string title, string directory, string extension, bool multiselect, AsyncCallback callback);

      [System.Runtime.InteropServices.DllImport("FileBrowser")]
      internal static extern void DialogOpenFolderPanelAsync(string title, string directory, bool multiselect, AsyncCallback callback);

      [System.Runtime.InteropServices.DllImport("FileBrowser")]
      internal static extern void DialogSaveFilePanelAsync(string title, string directory, string defaultName, string extension, AsyncCallback callback);
   }
}
#endif
// © 2019-2021 crosstales LLC (https://www.crosstales.com)