using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace TofuLab.CodeGenerator {
    public class EntityCodeGenerator : MonoBehaviour {
        private static string DirectoryPath () => "Assets/Scripts/";
        private static string CodePath (string entityName) => $"{DirectoryPath()}{entityName}.cs";
        private static string br = " ";
        private static string br4 = $"{br}{br}{br}{br}";
        private static string re = "\n";
        private static string code;

        [MenuItem ("CodeGenerator/Entity")]
        private static void GenerateCode () {
            var entitySO = Resources.Load ("SO/EntitySO") as EntitySO;

            var prop = $"{re}";
            entitySO.PropertySettingsObjectList.ForEach (item => {
                prop += GetProperty (item, entitySO.IsImmutable);
            });

            var args = $"{re}";
            entitySO.PropertySettingsObjectList.ForEach (item => {
                args += $"{br4}{br4}{item.Type.GetTypeName()}{br}{GetSnakeCase(item.Name)}";
                if (item != entitySO.PropertySettingsObjectList.Last ()) {
                    args += $",";
                }

                args += $"{re}";
            });

            var ctor = $"{re}";
            entitySO.PropertySettingsObjectList.ForEach (item => {
                ctor += $"{br4}{br4}{GetPascalCase(item.Name)}{br}={br}{GetSnakeCase(item.Name)};{re}";
            });

            code += $"public{br}class{br}{entitySO.Name}{br}{{";
            code += prop;
            code += $"{re}{br4}public{br}{entitySO.Name}{br}(";
            code += args;
            code += $"{br4}){br}{{";
            code += ctor;
            code += $"{br4}}}{re}}}";

            var isExist = Directory.Exists (DirectoryPath ());
            if (!isExist) Directory.CreateDirectory (DirectoryPath ());

            var assetPath = AssetDatabase.GenerateUniqueAssetPath (CodePath (entitySO.Name));

            File.WriteAllText (assetPath, code);
            AssetDatabase.Refresh ();

            print ($"Generated:{entitySO.Name}");
        }

        private static string GetProperty (PropertySettingsObject item, bool isImmutable) {
            var immutableAccessibility = isImmutable ? "private" : "";
            return $"{br4}public{br}{item.Type.GetTypeName()}{br}{GetPascalCase(item.Name)}{br}{{{br}get;{br}{immutableAccessibility}{br}set;{br}}}{re}";
        }

        private static string GetSnakeCase (string str) {
            var regex = new Regex ("[a-z][A-Z]");
            return regex.Replace (str, item => $"{item.Groups[0].Value[0]}_{item.Groups[0].Value[1]}").ToLower ();
        }

        private static string GetPascalCase (string str) {
            var regex1 = new Regex ("^[a-z]");
            var regex2 = new Regex ("_[a-zA-Z]");
            var tmp = regex1.Replace (str, item => $"{item.Groups[0].Value[0].ToString().ToUpper()}");
            return regex2.Replace (tmp, item => $"{item.Groups[0].Value[1].ToString().ToUpper()}");
        }
    }
}