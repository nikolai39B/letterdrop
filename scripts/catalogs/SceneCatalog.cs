using Godot;
using LetterDrop.Debug;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterDrop
{
	public enum SceneType
	{
		//-- GENERAL
		INVALID = -1,
		NULL = 0,

		//-- FIELD
		TILE,
		FIELD_COLUMN
	}

	/// <summary>
	/// A catalog of preregistered scenes available for instantiation.
	/// </summary>
	internal static class SceneCatalog
	{
		//-- STATIC CONSTRUCTOR

		static SceneCatalog()
		{
            ScenePaths = new Dictionary<SceneType, SceneLoader>
            {
				{ SceneType.TILE, new SceneLoader("res://scenes/tile.tscn") },
                { SceneType.FIELD_COLUMN, new SceneLoader("res://scenes/field_column.tscn") }
            };

        }


        //-- INSTANTIATION

        /// <summary>
        /// Instantiates an instance of the given scene.
        /// </summary>
        /// <typeparam name="T">The type of element to instantiate</typeparam>
        /// <param name="sceneType">The scene to instantiate</param>
        /// <returns></returns>
        public static T Instantiate<T>(SceneType sceneType) where T : class
		{
			// Validate
			if (DebugUtils.AssertFalse(!ScenePaths.ContainsKey(sceneType)))
			{
				return null;
			}

			// Instantiate
            SceneLoader loader = ScenePaths[sceneType];
			return loader?.PackedScene?.Instantiate<T>();
        }


        //-- STATIC PROPERTIES

		/// <summary>
		/// The map of scene type to the corresponding loader
		/// </summary>
        private static Dictionary<SceneType, SceneLoader> ScenePaths { get; set; }

    }

    /// <summary>
    /// Object which loads on demand the appropriate packed scene
    /// </summary>
    /// <param name="path">The resource path of the packed scene</param>
    class SceneLoader(String path)
    {
        //-- PROPERTIES

        /// <summary>
        /// The resource path of the packed scene
        /// </summary>
        public String Path { get; private set; } = path;

        /// <summary>
        /// The loaded packed scene
        /// </summary>
        public PackedScene PackedScene
        {
            get
            {
                // Load the packed scene if necessary
                if (_packedScene == null)
                {
                    _packedScene = GD.Load<PackedScene>(Path);
                }
                return _packedScene;
            }
        }
        private PackedScene _packedScene = null;
    }
}
