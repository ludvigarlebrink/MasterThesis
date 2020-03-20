using Photon.Pun;
using UnityEngine;

public enum Space
{
    Local,
    World
}

[RequireComponent(typeof(PhotonView))]
public class TransformSubject : MonoBehaviour, IPunObservable
{
    public Space m_Space = Space.Local;

    public bool m_SynchronizePosition = true;
    public bool m_SynchronizeRotation = true;
    public bool m_SynchronizeScale = false;

    private float m_Distance;
    private float m_Angle;
    private PhotonView m_PhotonView;
    private Vector3 m_Direction;
    private Vector3 m_NetworkPosition;
    private Vector3 m_StoredPosition;
    private Quaternion m_NetworkRotation;
    private bool m_FirstTake = false;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            if (m_SynchronizePosition)
            {
                if (m_Space == Space.Local)
                {
                    m_Direction = transform.localPosition - m_StoredPosition;
                    m_StoredPosition = transform.localPosition;
                    stream.SendNext(transform.localPosition);
                    stream.SendNext(m_Direction);
                }
                else
                {
                    m_Direction = transform.position - m_StoredPosition;
                    m_StoredPosition = transform.position;
                    stream.SendNext(transform.position);
                    stream.SendNext(m_Direction);
                }
            }

            if (m_SynchronizeRotation)
            {
                if (m_Space == Space.Local)
                {
                    stream.SendNext(transform.localRotation);
                }
                else
                {
                    stream.SendNext(transform.rotation);
                }
            }

            if (m_SynchronizeScale)
            {
                stream.SendNext(transform.localScale);
            }
        }
        else
        {
            if (m_SynchronizePosition)
            {
                m_NetworkPosition = (Vector3)stream.ReceiveNext();
                m_Direction = (Vector3)stream.ReceiveNext();

                if (m_FirstTake)
                {
                    if (m_FirstTake)
                    {
                        transform.localPosition = m_NetworkPosition;
                    }
                    else
                    {
                        transform.position = m_NetworkPosition;
                    }

                    m_Distance = 0.0f;
                }
                else
                {
                    if (m_Space == Space.Local)
                    {
                        float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
                        m_NetworkPosition += m_Direction * lag;
                        m_Distance = Vector3.Distance(transform.localPosition, m_NetworkPosition);
                    }
                    else
                    {
                        float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
                        m_NetworkPosition += m_Direction * lag;
                        m_Distance = Vector3.Distance(transform.position, m_NetworkPosition);
                    }
                }
            }

            if (m_SynchronizeRotation)
            {
                m_NetworkRotation = (Quaternion)stream.ReceiveNext();

                if (m_FirstTake)
                {
                    m_Angle = 0.0f;
                    if (m_Space == Space.Local)
                    {
                        transform.localRotation = m_NetworkRotation;
                    }
                    else
                    {
                        transform.rotation = m_NetworkRotation;
                    }
                }
                else
                {
                    if (m_Space == Space.Local)
                    {
                        m_Angle = Quaternion.Angle(transform.localRotation, m_NetworkRotation);
                    }
                    else
                    {
                        m_Angle = Quaternion.Angle(transform.rotation, m_NetworkRotation);
                    }
                }
            }

            if (m_SynchronizeScale)
            {
                transform.localScale = (Vector3)stream.ReceiveNext();
            }

            if (m_FirstTake)
            {
                m_FirstTake = false;
            }
        }
    }

    private void Awake()
    {
        m_PhotonView = GetComponent<PhotonView>();

        if (m_Space == Space.Local)
        {
            m_StoredPosition = transform.localPosition;
        }
        else
        {
            m_StoredPosition = transform.position;
        }

        m_NetworkPosition = Vector3.zero;

        m_NetworkRotation = Quaternion.identity;
    }

    private void OnEnable()
    {
        m_FirstTake = true;
    }

    private void Update()
    {
        if (!m_PhotonView.IsMine)
        {
            if (m_Space == Space.Local)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, m_NetworkPosition, m_Distance * (1.0f / PhotonNetwork.SerializationRate));
                transform.localRotation = Quaternion.RotateTowards(transform.localRotation, m_NetworkRotation, m_Angle * (1.0f / PhotonNetwork.SerializationRate));
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, m_NetworkPosition, m_Distance * (1.0f / PhotonNetwork.SerializationRate));
                transform.rotation = Quaternion.RotateTowards(transform.rotation, m_NetworkRotation, m_Angle * (1.0f / PhotonNetwork.SerializationRate));
            }
        }
    }
}