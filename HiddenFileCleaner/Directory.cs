using System.Collections.Generic;
using System.Linq;

namespace HiddenFileCleaner
{
    // 参考：EnumerateFiles(?, ?, AllDirectories) メソッドで、アクセス権エラーを回避する - Qiita
    //       https://qiita.com/otagaisama-1/items/8e5022367ee13a0a0193

    public class Directory
    {
        public static IEnumerable<string> SafeEnumerateFilesInAllDirectories(string path)
        {
            return SafeEnumerateFilesInAllDirectories(path, "*");
        }

        public static IEnumerable<string> SafeEnumerateFilesInAllDirectories(string path, string searchPattern)
        {
            var files = Enumerable.Empty<string>();
            try
            {
                files = System.IO.Directory.EnumerateFiles(path, searchPattern);
            }
            catch (System.UnauthorizedAccessException)
            {
            }
            try
            {
                files = System.IO.Directory.EnumerateDirectories(path)
                    .Aggregate<string, IEnumerable<string>>(
                        files,
                        (a, v) => a.Union(SafeEnumerateFilesInAllDirectories(v, searchPattern))
                    );
            }
            catch (System.UnauthorizedAccessException)
            {
            }

            return files;
        }
    }
}
