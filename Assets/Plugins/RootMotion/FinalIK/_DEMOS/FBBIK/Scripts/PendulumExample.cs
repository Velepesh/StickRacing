using UnityEngine;
using System.Collections;
using RootMotion.FinalIK;



namespace RootMotion.Demos {

	/// <summary>
	/// Making a character hold on to a target and swing about it while maintaining his animation.
	/// </summary>
	/// 
	[RequireComponent(typeof(InteractionSystem))]
	[RequireComponent(typeof(FullBodyBipedIK))]
	public class PendulumExample : MonoBehaviour 
	{
		[Tooltip("The master weight of this script.")]
		[Range(0f, 1f)] public float weight = 1f;

		[Tooltip("Multiplier for the distance of the root to the target.")]
		public float hangingDistanceMlp = 1.3f;

		[Tooltip("Where does the root of the character land when weight is blended out?")]
		[HideInInspector] public Vector3 rootTargetPosition;

		[Tooltip("How is the root of the character rotated when weight is blended out?")]
		[HideInInspector] public Quaternion rootTargetRotation;

		public InteractionObject _interLeftObject;
		public InteractionObject _interRightObject;

		public FullBodyBipedEffector _effectorLeft;
		public FullBodyBipedEffector _effectorRight;

        public Transform _startLeftTarget;
        public Transform _startRightTarget;
        public Transform _leftHand;
        public Transform _rightHand;
        public Transform leftFootTarget;
        public Transform rightFootTarget;
        public Transform pelvisTarget;
        public Transform bodyTarget;
        public Transform headTarget;
        public Vector3 pelvisDownAxis = Vector3.right;

        private FullBodyBipedIK ik;
		private Quaternion rootRelativeToPelvis;
		private Vector3 pelvisToRoot;
		private float lastWeight;

		private InteractionSystem _interactionSystem;
		private bool _isCurrentLeftHand;

        private Transform _currentLeft;
        private Transform _currentRight;
        private void Start() 
		{
            _currentLeft = _startLeftTarget;
            _currentRight = _startRightTarget;
            _isCurrentLeftHand = true;

			_interactionSystem = GetComponent<InteractionSystem>();
			ik = GetComponent<FullBodyBipedIK>();
			// Connect the left hand to the target		

			//target.rotation = targetRotation;

			// Remember the rotation of the root relative to the pelvis
			rootRelativeToPelvis = Quaternion.Inverse(pelvisTarget.rotation) * transform.rotation;

			// Remember the position of the root relative to the pelvis
			pelvisToRoot = Quaternion.Inverse(ik.references.pelvis.rotation) * (transform.position - ik.references.pelvis.position);

			rootTargetPosition = transform.position;
			rootTargetRotation = transform.rotation;

			lastWeight = weight;
			
            ConnectHand(_leftHand, _startLeftTarget, _startLeftTarget);
            ConnectHand(_rightHand, _startRightTarget, _startRightTarget);

			StartCoroutine(StartInteraction(_interLeftObject, _effectorLeft));
			StartCoroutine(StartInteraction(_interRightObject, _effectorRight));
        }
      
        public void StartInteractionWithStick(GameObject stick, Transform connectionPoint)
		{
			InteractionObject interaction = stick.GetComponent<InteractionObject>();

            if (_isCurrentLeftHand)
            {
                var joint = _currentLeft.gameObject.GetComponent<FixedJoint>();
                joint.connectedBody = null;

                ConnectHand(_leftHand, stick.transform, connectionPoint);
                StartCoroutine(StartInteraction(interaction, _effectorLeft));

                _isCurrentLeftHand = false;

                //_currentLeft = stick.transform;
            }
            else
            {
                var joint = _currentRight.gameObject.GetComponent<FixedJoint>();
                joint.connectedBody = null;

                ConnectHand(_rightHand, stick.transform, connectionPoint);
                StartCoroutine(StartInteraction(interaction, _effectorRight));

                _isCurrentLeftHand = true;

               // _currentRight = stick.transform;
            }
        }

        private void ConnectHand(Transform hand, Transform target, Transform connectionPoint)
		{
			Quaternion targetRotation = connectionPoint.transform.rotation;
           // StartCoroutine(Move(hand, target));
            hand.gameObject.transform.position = connectionPoint.position;
            Debug.Log(hand.gameObject.transform.position + " hand.gameObject.transform.position");
            Debug.Log(connectionPoint.position + " connectionPoint.position");
            FixedJoint j = target.gameObject.AddComponent<FixedJoint>();
			j.connectedBody = hand.GetComponent<Rigidbody>();
            j.enablePreprocessing = false;

			target.GetComponent<Rigidbody>().MoveRotation(targetRotation);
        }

        private IEnumerator Move(Transform hand, Transform target)
        {
            //while (hand.gameObject.transform.position != target.position)
            //{

            //}

            //float elapsedTime = 0;

            while (hand.gameObject.transform.position != target.position)
            {
               // hand.transform.position = Vector3.Lerp(hand.transform.position, target.position, 10 * Time.deltaTime);
                Debug.Log("TRA");
                //elapsedTime += Time.deltaTime;
                yield return null;
            }

            // print("Attaching to " + handPosition.name);
            yield return null;
        }

        private IEnumerator StartInteraction(InteractionObject interaction, FullBodyBipedEffector effector)
        {
			yield return new WaitForSeconds(0.05f);

			_interactionSystem.StartInteraction(effector, interaction, true);
		}
		private void LateUpdate()
        { 
			// Set effector weights
			if (weight > 0f)
            {
                ik.solver.leftHandEffector.positionWeight = weight;
                ik.solver.leftHandEffector.rotationWeight = weight;

                ik.solver.rightHandEffector.positionWeight = weight;
                ik.solver.rightHandEffector.rotationWeight = weight;
            }
            else
            {
                rootTargetPosition = transform.position;
                rootTargetRotation = transform.rotation;

                if (lastWeight > 0f)
                {
                    ik.solver.leftHandEffector.positionWeight = 0f;
                    ik.solver.leftHandEffector.rotationWeight = 0f;

                    ik.solver.rightHandEffector.positionWeight = 0f;
                    ik.solver.rightHandEffector.rotationWeight = 0f;
                }
            }

            lastWeight = weight;

            if (weight <= 0f)
                return;

            //// Position the character relative to the ragdoll pelvis
            transform.position = Vector3.Lerp(rootTargetPosition, pelvisTarget.position + pelvisTarget.rotation * pelvisToRoot * hangingDistanceMlp, weight);
            Debug.Log(pelvisTarget.position + pelvisTarget.rotation * pelvisToRoot * hangingDistanceMlp);
            //// Rotate the character to the ragdoll pelvis
            transform.rotation = Quaternion.Lerp(rootTargetRotation, pelvisTarget.rotation * rootRelativeToPelvis, weight);

            // Set ik effector positions
            ik.solver.leftHandEffector.position = _leftHand.position;
            ik.solver.leftHandEffector.rotation = _leftHand.rotation;

            ik.solver.rightHandEffector.position = _rightHand.position;
            ik.solver.rightHandEffector.rotation = _rightHand.rotation;



            // Get the normal hanging direction
            Vector3 dir = ik.references.pelvis.rotation * pelvisDownAxis;

            // Rotating the limbs
            // Get the rotation from normal hangind direction to the right arm ragdoll direction
            //Quaternion rightArmRot = Quaternion.FromToRotation(dir, _rightHand.position - headTarget.position);
            //// Rotate the right arm by that offset
            //ik.references.rightUpperArm.rotation = Quaternion.Lerp(Quaternion.identity, rightArmRot, weight) * ik.references.rightUpperArm.rotation;

            Quaternion leftLegRot = Quaternion.FromToRotation(dir, leftFootTarget.position - bodyTarget.position);
            ik.references.leftThigh.rotation = Quaternion.Lerp(Quaternion.identity, leftLegRot, weight) * ik.references.leftThigh.rotation;

            Quaternion rightLegRot = Quaternion.FromToRotation(dir, rightFootTarget.position - bodyTarget.position);
            ik.references.rightThigh.rotation = Quaternion.Lerp(Quaternion.identity, rightLegRot, weight) * ik.references.rightThigh.rotation;
        }
    }
}