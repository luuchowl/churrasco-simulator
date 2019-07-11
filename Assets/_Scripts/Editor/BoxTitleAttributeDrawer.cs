using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(BoxTitleAttribute))]
public class BoxTitleAttributeDrawer : DecoratorDrawer {

	BoxTitleAttribute boxTitleAttribute;

	//Override this fto define the custom drawing
	public override void OnGUI(Rect position) {
		boxTitleAttribute = (BoxTitleAttribute)attribute;
		string title = boxTitleAttribute.title;
		float posY = 5;
		Color boxColor = boxTitleAttribute.color; ;

		if (EditorApplication.isPlaying) { //If we're on play mode, the box will be a little transparent
			boxColor.a *= .5f;
		}

		GUI.color = boxColor;

		position.y += posY;

		GUI.Box(new Rect(position.xMin, position.yMin, position.width, 20), title);

		//Return the color to normal
		GUI.color = Color.white;
	}

	//By overrinding this, we can customize the height our attribute will take
	public override float GetHeight() {
		return 30f;
	}
}
