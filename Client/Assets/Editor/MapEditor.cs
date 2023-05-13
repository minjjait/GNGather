using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MapEditor
{

#if UNITY_EDITOR

	// % (Ctrl), # (Shift), & (Alt)
	[MenuItem("Tools/GenerateMap %#g")]
	private static void GenerateMap()
	{
		GenerateByPath("Assets/Resources/Map");
		GenerateByPath("../Common/MapData");
	}

	//각 지열별 번호를 추가한 맵 text파일 생성
	private static void GenerateByPath(string pathPrefix)
	{
		GameObject[] gameObjects = Resources.LoadAll<GameObject>("Prefabs/Map");

		foreach (GameObject go in gameObjects)
		{
			Tilemap tmBase = Util.FindChild<Tilemap>(go, "Tilemap_Base", true);

			//지역생성
			Transform regions = go.transform.GetChild(1);
			Tilemap[] tmRegion = new Tilemap[42];
			for(int i = 0; i < regions.childCount; i++)
            {
				tmRegion[i] = regions.GetChild(i).GetComponent<Tilemap>();
            }

			Tilemap tm = Util.FindChild<Tilemap>(go, "Tilemap_Collision", true);
			
			using (var writer = File.CreateText($"{pathPrefix }/{go.name}.txt"))
			{
				writer.WriteLine(tmBase.cellBounds.xMin);
				writer.WriteLine(tmBase.cellBounds.xMax);
				writer.WriteLine(tmBase.cellBounds.yMin);
				writer.WriteLine(tmBase.cellBounds.yMax);

				for (int y = tmBase.cellBounds.yMax; y >= tmBase.cellBounds.yMin; y--)
				{
					for (int x = tmBase.cellBounds.xMin; x <= tmBase.cellBounds.xMax; x++)
					{
						bool haveTile = false;
						TileBase tile = tm.GetTile(new Vector3Int(x, y, 0));

						TileBase[] regionTiles = new TileBase[42];
						{//지역 타일 획득
							for (int i = 0; i < tmRegion.Length; i++)
                            {
								regionTiles[i] = tmRegion[i].GetTile(new Vector3Int(x, y, 0));
                            }
                        }

						if (tile != null)
							writer.Write("999 ");
                        else
                        {
							for(int i = 0; i < regionTiles.Length; i++)
                            {
								if(regionTiles[i] != null)
                                {
									writer.Write(tmRegion[i].name + " ");
									haveTile = true;
									break;
                                }
                            }
							if (haveTile == false)
								writer.Write("0 ");
						}
					}
					writer.WriteLine();
				}
			}
		}
	}

#endif

}
