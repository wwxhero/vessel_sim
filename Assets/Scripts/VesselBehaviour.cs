using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TestMySpline;

public class VesselBehaviour : MonoBehaviour
{
	public KnotBehavior [] m_knots;
	public int m_nIerps;
	public bool m_debug = true;
	Vector3 [] m_pknotsM;
	int m_nIerpsM;
	bool m_debugM = true;	
	Vector3 [] m_pIerps;
	readonly float c_distEpsilon = 0.001f;


	bool UpdateSpline()
	{
		int 	n_knot = m_knots.Length;
		if (n_knot > 3)
		{
			float[]	i_knot = new float[n_knot];
			float[]	x_knot = new float[n_knot];
			float[]	y_knot = new float[n_knot];
			float[]	z_knot = new float[n_knot];
			m_pknotsM = new Vector3[n_knot];
			for (int i = 0; i < n_knot; i ++)
			{
				m_pknotsM[i] = m_knots[i].transform.position;
				i_knot[i] = i;
				x_knot[i] = m_pknotsM[i].x;
				y_knot[i] = m_pknotsM[i].y;
				z_knot[i] = m_pknotsM[i].z;

			}
		
			float[] i_ierps = new float[m_nIerps];
			float ierp_step = (float)(n_knot-1)/(float)(m_nIerps-1);
			i_ierps[0] = 0;
			for (int i = 1; i < m_nIerps; i ++)
			{
				i_ierps[i] = i_ierps[i-1] + ierp_step;
			}
			float[] x_ierps = CubicSpline.Compute(i_knot, x_knot, i_ierps);
			float[] y_ierps = CubicSpline.Compute(i_knot, y_knot, i_ierps);
			float[] z_ierps = CubicSpline.Compute(i_knot, z_knot, i_ierps);
		
			m_pIerps = new Vector3[m_nIerps];
			for (int i = 0; i < m_nIerps; i ++)
			{
				m_pIerps[i].x = x_ierps[i];
				m_pIerps[i].y = y_ierps[i];
				m_pIerps[i].z = z_ierps[i];
			}
			return true;
		}
		else
			return false;
	}

	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		if (m_debug)
		{
			bool knot_moved = (null == m_pknotsM
							|| m_nIerps != m_nIerpsM);
			for (int i = 0
				; !knot_moved
				 && i < m_knots.Length
				; i ++) // exists m_knotes -> m_knotes[i] is moved for any i
			{
				knot_moved = (Vector3.Magnitude(m_knots[i].transform.position - m_pknotsM[i]) > c_distEpsilon);
			}
			if (knot_moved)
				UpdateSpline();
			Color clr = new Color(1.0f, 0f, 0f);
			for (int i = 1; i < m_nIerps; i ++)		
			{
				Debug.DrawLine(m_pIerps[i-1], m_pIerps[i], clr);
			}
		}

        if (m_debugM != m_debug)
		{
			foreach (var knot in m_knots)
			{
                var render = knot.GetComponent<MeshRenderer>();
                render.enabled = m_debug;
			}
		}

        m_debugM = m_debug;
        m_nIerpsM = m_nIerps;

	}
}
