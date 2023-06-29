using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class RectSizeFitter : UIBehaviour, ILayoutSelfController
{
	[SerializeField] RectTransform _rectToCopy;

	[Space]
	
	[SerializeField] bool _fitHorizontal;
	[SerializeField] bool _fitVertical;
	
	RectTransform m_rectTransform;
	RectTransform _rectTransform
	{
		get
		{
			if(m_rectTransform == null) m_rectTransform = GetComponent<RectTransform>();
			return m_rectTransform;
		}
	}
	
	protected override void OnCanvasHierarchyChanged()
	{
		SetDirty();
	}
	
	void CalculateAlongAxis(int axis)
	{
		if (!_rectToCopy) return;

		bool fit = axis == 0 ? _fitHorizontal : _fitVertical;

		if(fit)
		{
			RectTransform.Axis axisOfScale = (RectTransform.Axis)axis;
			_rectTransform.SetSizeWithCurrentAnchors(axisOfScale, GetSizeOfAxis(_rectToCopy, axisOfScale));
		}
	}

	public void SetLayoutHorizontal()
	{
		CalculateAlongAxis(0);
	}

	public void SetLayoutVertical()
	{
		CalculateAlongAxis(1);
	}

	float GetSizeOfAxis(RectTransform transform, RectTransform.Axis axis)
	{
		switch (axis)
		{
			default:
			case RectTransform.Axis.Horizontal:
				return transform.sizeDelta.x;
			case RectTransform.Axis.Vertical:
				return transform.sizeDelta.y;
		}
	}

	protected void SetDirty()
	{
		if (!IsActive())
			return;

		LayoutRebuilder.MarkLayoutForRebuild(_rectTransform);
	}
}
