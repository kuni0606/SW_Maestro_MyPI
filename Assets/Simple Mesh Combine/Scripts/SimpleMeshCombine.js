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
//Add the script to the parent gameObject, then click combine

@script AddComponentMenu("Simple Mesh Combine")
#pragma strict
#pragma downcast
	var combinedGameOjects:GameObject[];	//Stores gameObjects that has been merged, mesh renderer disabled
	var combined:GameObject;				//Stores the combined mesh gameObject
	var meshName:String = "Combined_Meshes";//Asset name when saving as prefab
	var _advanced:boolean;					//Toggles advanced features
	var _savedPrefab:boolean;				//Used when checking if this mesh has been saved to prefab (saving the same mesh twice generates error)
	var _generateLightmapUV:boolean;		//Toggles secondary UV map generation
	
	function EnableRenderers(e:boolean) {	
    	for (var i:int = 0; i < combinedGameOjects.length; i++){
        	combinedGameOjects[i].GetComponent.<Renderer>().enabled = e;
   		}  
	}
	
	function FindEnabledMeshes() 
	{//Returns a meshFilter[] list of all renderer enabled meshfilters(so that it does not merge disabled meshes, useful when there are invisible box colliders)
		var renderers:MeshFilter[];
		var count:int;
		renderers = transform.GetComponentsInChildren.<MeshFilter>();
			
		for (var i:int = 0; i < renderers.length; i++)
		{//count all the enabled meshrenderers in children
			if(renderers[i].GetComponent(MeshRenderer) && renderers[i].GetComponent(MeshRenderer).enabled)
				count++;
		}
		var meshfilters = new MeshFilter[count];//creates a new array with the correct length
		count = 0;
		for (var ii:int = 0; ii < renderers.length; ii++)
		{//adds all enabled meshes to the array
			if(renderers[ii].GetComponent(MeshRenderer) && renderers[ii].GetComponent(MeshRenderer).enabled){
				meshfilters[count] = renderers[ii];
				count++;
			}
		}
		return meshfilters;
	}
	//// COMBINE FRAGMENTS TO A SINGLE MESH
function CombineMeshes() {
	
		
		var combo:GameObject = new GameObject();
		combo.name = "_Combined Mesh [" + name + "]";
		combo.gameObject.AddComponent(MeshFilter);
		combo.gameObject.AddComponent(MeshRenderer);
		var meshFilters:MeshFilter[];
		meshFilters = FindEnabledMeshes();
		var materials: ArrayList = new ArrayList();
		var combineInstanceArrays: ArrayList = new ArrayList();
		combinedGameOjects = new GameObject[meshFilters.length];
		for (var i = 0; i < meshFilters.length; i++) {
			var meshFilterss: MeshFilter[] = meshFilters[i].GetComponentsInChildren.<MeshFilter>();
			
			
			combinedGameOjects[i] = meshFilters[i].gameObject;
			
			for (var meshFilter: MeshFilter in meshFilterss) {
				var meshRenderer: MeshRenderer = meshFilter.GetComponent(MeshRenderer);
				
					
					meshFilters[i].transform.gameObject.GetComponent(Renderer).enabled = false;

					for (var o: int = 0; o < meshFilter.sharedMesh.subMeshCount; o++) {
						var materialArrayIndex: int = Contains(materials, meshRenderer.sharedMaterials[o].name);
						if (materialArrayIndex == -1) {
							materials.Add(meshRenderer.sharedMaterials[o]);
							materialArrayIndex = materials.Count - 1;
						}
						combineInstanceArrays.Add(new ArrayList());
						var combineInstance: CombineInstance = new CombineInstance();
						combineInstance.transform = meshRenderer.transform.localToWorldMatrix;
						combineInstance.subMeshIndex = o;
						combineInstance.mesh = meshFilter.sharedMesh;
						(combineInstanceArrays[materialArrayIndex] as ArrayList).Add(combineInstance);
					}
				}
			
		}	
		var meshes: Mesh[] = new Mesh[materials.Count];
		var combineInstances: CombineInstance[] = new CombineInstance[materials.Count];
		for (var m: int = 0; m < materials.Count; m++) {
			var combineInstanceArray: CombineInstance[] = (combineInstanceArrays[m] as ArrayList).ToArray(typeof(CombineInstance)) as CombineInstance[];
			meshes[m] = new Mesh();
			meshes[m].CombineMeshes(combineInstanceArray, true, true);
			combineInstances[m] = new CombineInstance();
			combineInstances[m].mesh = meshes[m];
			combineInstances[m].subMeshIndex = 0;
		}		
		combo.GetComponent(MeshFilter).sharedMesh = new Mesh();
		combo.GetComponent(MeshFilter).sharedMesh.CombineMeshes(combineInstances, false, false);

		for (var mesh: Mesh in meshes) {
			mesh.Clear();
			DestroyImmediate(mesh);
		}
		var meshRendererCombine: MeshRenderer = combo.GetComponent(MeshFilter).GetComponent(MeshRenderer);
		if (!meshRendererCombine) meshRendererCombine = gameObject.AddComponent(MeshRenderer);
		var materialsArray: Material[] = materials.ToArray(typeof(Material)) as Material[];
		meshRendererCombine.materials = materialsArray;	
		combined = combo.gameObject;
		#if UNITY_EDITOR
		if(_generateLightmapUV){
   			Unwrapping.GenerateSecondaryUVSet(combined.GetComponent(MeshFilter).sharedMesh);
	   		combined.isStatic = true;
	   	}
	   	#endif
	    EnableRenderers(false);
	    combo.transform.parent = transform;
	
}
function Contains(l: ArrayList, n: String) {
	for (var i: int = 0; i < l.Count; i++) {
		if ((l[i] as Material).name == n) {
			return i;
		}
	}
	return -1;
}

//// OLD VERSION

//	function combineMeshes () 
//	{//Combines meshes
//		var combinedGameOjects:GameObject = new GameObject();
//		combinedGameOjects.AddComponent(MeshFilter);
//		combinedGameOjects.AddComponent(MeshRenderer);		
//		var meshFilters;
//		meshFilters = FindEnabledMeshes();
//	    var combine: CombineInstance[] = new CombineInstance[meshFilters.length];
//	      
//	    Debug.Log("Simple Mesh Combine: Combined " + meshFilters.length + " Meshes");
//	      
//	    combinedGameOjects = new GameObject[meshFilters.length];      
//	    for (var i:int = 0; i < meshFilters.length; i++)
//	    {
//	    	combinedGameOjects.GetComponent(MeshRenderer).sharedMaterial = meshFilters[i].transform.gameObject.GetComponent(MeshRenderer).sharedMaterial;
//	    	combinedGameOjects[i] = meshFilters[i].gameObject;
//	        combine[i].mesh = meshFilters[i].transform.GetComponent(MeshFilter).sharedMesh;
//	        combine[i].transform = meshFilters[i].transform.localToWorldMatrix;  		
//	    }
//	    
//	    combinedGameOjects.GetComponent(MeshFilter).mesh = new Mesh();
//	    combinedGameOjects.GetComponent(MeshFilter).sharedMesh.CombineMeshes(combine);
//	    if(_generateLightmapUV){
//	   		Unwrapping.GenerateSecondaryUVSet(combinedGameOjects.GetComponent(MeshFilter).sharedMesh);
//	   		combinedGameOjects.isStatic = true;
//	   	}
//	   		
//	    combinedGameOjects.name = "_Combined Mesh [" + name + "]";
//	    combined = combinedGameOjects.gameObject;
//	    EnableRenderers(false);
//	    combinedGameOjects.transform.parent = transform;
//    }	