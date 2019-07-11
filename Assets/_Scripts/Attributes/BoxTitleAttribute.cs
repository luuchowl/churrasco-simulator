using UnityEngine;

public class BoxTitleAttribute : PropertyAttribute {
	public string title;
	public Color color;

	public BoxTitleAttribute(string _title) {
		title = _title;
		color = Color.white;
	}

	public BoxTitleAttribute(string _title, float r, float g, float b) {
		title = _title;
		color = new Color(r, g, b);
	}
}
