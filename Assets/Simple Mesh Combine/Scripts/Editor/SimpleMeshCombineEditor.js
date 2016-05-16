/****************************************
	Simple Mesh Combine v1.301			
	Copyright 2014 Unluck Software	
 	www.chemicalbliss.com
 	
 	Change Log
 		v1.1
 		Added naming and prefab save option	
 		v1.2
 		Added lightmap support		
 		v1.3
 		Added multiple material support
 			v1.301
 			Fixed compile error trying to unwrap UV in game mode																															
*****************************************/
import System.IO;
@CustomEditor(SimpleMeshCombine)

public class SimpleMeshCombineEditor extends Editor {
	
    override function OnInspectorGUI () {
   // 	DrawDefaultInspector ();
   		GUILayout.Space(10);
		GUILayout.Label("*All meshes must have same material*");	
		GUILayout.Space(10);
		if(!target.combined){
		target._generateLightmapUV = EditorGUILayout.Toggle("Generate Ligthmap UV's", target._generateLightmapUV);
		GUILayout.Label("Combine all Mesh Renderer enabled meshes");
			if(GUILayout.Button("Combine")) {
				if(target.transform.childCount > 1) target.CombineMeshes();
			}   
       }else{
			GUILayout.Label("Decombine all previously combined meshes");
			if(GUILayout.Button("Release")) {
				target.EnableRenderers(true);
				target._savedPrefab = false;
					if(target.combined)
						DestroyImmediate(target.combined);
			}
        }
        if(target.combined && !target._savedPrefab)
        	target._advanced = EditorGUILayout.Toggle("Advanced Features", target._advanced);
        if(target.combined && target._advanced && !target._savedPrefab){
        	if(GUILayout.Button("Save Mesh")) {
        		var n:String = target.meshName;
        		if(System.IO.Directory.Exists("Assets/Simple Mesh Combine/Saved Meshes/")){
        		if(!System.IO.File.Exists("Assets/Simple Mesh Combine/Saved Meshes/"+target.meshName+".asset")){     	
        			AssetDatabase.CreateAsset(target.combined.GetComponent(MeshFilter).sharedMesh, "Assets/Simple Mesh Combine/Saved Meshes/"+n+".asset");
        			target._advanced = false;
        			target._savedPrefab = true;
        			Debug.Log("Saved Assets/Simple Mesh Combine/Saved Meshes/"+n+".asset");
        		}else{
        			Debug.Log(target.meshName+".asset" + " already exists, please change the name");
        		}
        		
        		}else{
        			Debug.Log("Missing Folder: Assets/Simple Mesh Combine/Saved Meshes/");
        		}
        	}
        	target.meshName = GUILayout.TextField(target.meshName);
        }
        if (GUI.changed){
                EditorUtility.SetDirty(target);
        }
   }
   
}