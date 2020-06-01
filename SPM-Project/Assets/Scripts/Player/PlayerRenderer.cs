using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Viktor Dahlberg
/// Materials are stored in arrays because skinned mesh renderers need an array of materials assigned.
/// The reason we don't just change the rendering mode of a single material is because the Standard shader is a little quirky like that.
public class PlayerRenderer : MonoBehaviour {

	[SerializeField][Tooltip("and starting mode.")] private RenderMode currentMode;
	[SerializeField] private RenderModeItem[] renderModes;
	[SerializeField] private SkinnedMeshRenderer[] playerRenderers;

	private Dictionary<RenderMode, Material[]> renderModeLookup;
	
	private void Awake() {
		renderModeLookup = new Dictionary<RenderMode, Material[]>();
		for (int i = 0; i < renderModes.Length; i++) {
			renderModeLookup.Add(renderModes[i].Keyword, renderModes[i].Materials);
		}
		SetMaterialOnRenderers(renderModeLookup[currentMode]);
	}

	/// <summary>
	/// Changes the way the player material should be rendered.
	/// </summary>
	/// <param name="renderMode">The way the player should be rendered.</param>
	public void SetRenderMode(RenderMode renderMode) {
		if (currentMode == renderMode) return;
		currentMode = renderMode;
		SetMaterialOnRenderers(renderModeLookup[currentMode]);
	}

	private void SetMaterialOnRenderers(Material[] material) {
		for (int i = 0; i < playerRenderers.Length; i++) {
			playerRenderers[i].materials = material;
		}
	}

}

[System.Serializable]
public struct RenderModeItem {
	public RenderMode Keyword;
	public Material[] Materials;
}

public enum RenderMode {
	Opaque,
	Transparent
}