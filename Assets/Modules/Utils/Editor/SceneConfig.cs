namespace Utils.Project.Scripts.Modules.Utils.Editor
{
    public class SceneConfig
    {
        public string Name { get; }
        public string Path { get; }
        
        public SceneConfig(string name, string path)
        {
            Name = name;
            Path = path;
        }
    }
}