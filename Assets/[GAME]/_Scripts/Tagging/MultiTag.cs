using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiTag : MonoBehaviour
{
	public List<Tag> tags = new List<Tag>();

	public bool ContainsTag(Tag tag)
	{
		for (int i = 0; i < tags.Count; i++)
		{
			if (tags[i] == tag)
			{
				return true;
			}
		}

		return false;
	}

	public bool CompareAllTags(Tag[] _tags)
	{
		foreach (var tag in _tags)
		{
			if (!tags.Contains(tag))
				return false;
		}

		return true;
	}
}
