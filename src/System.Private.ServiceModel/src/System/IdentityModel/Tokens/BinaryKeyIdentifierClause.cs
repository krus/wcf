// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Globalization;
using System.ServiceModel;

namespace System.IdentityModel.Tokens
{
    public abstract class BinaryKeyIdentifierClause : SecurityKeyIdentifierClause
    {
        private readonly byte[] _identificationData;

        protected BinaryKeyIdentifierClause(string clauseType, byte[] identificationData, bool cloneBuffer)
            : this(clauseType, identificationData, cloneBuffer, null, 0)
        {
        }

        protected BinaryKeyIdentifierClause(string clauseType, byte[] identificationData, bool cloneBuffer, byte[] derivationNonce, int derivationLength)
            : base(clauseType, derivationNonce, derivationLength)
        {
            if (identificationData == null)
            {
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("identificationData"));
            }
            if (identificationData.Length == 0)
            {
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("identificationData", SR.LengthMustBeGreaterThanZero));
            }

            if (cloneBuffer)
            {
                _identificationData = SecurityUtils.CloneBuffer(identificationData);
            }
            else
            {
                _identificationData = identificationData;
            }
        }

        public byte[] GetBuffer()
        {
            return SecurityUtils.CloneBuffer(_identificationData);
        }

        protected byte[] GetRawBuffer()
        {
            return _identificationData;
        }

        public override bool Matches(SecurityKeyIdentifierClause keyIdentifierClause)
        {
            BinaryKeyIdentifierClause that = keyIdentifierClause as BinaryKeyIdentifierClause;
            return ReferenceEquals(this, that) || (that != null && that.Matches(_identificationData));
        }

        public bool Matches(byte[] data)
        {
            return Matches(data, 0);
        }

        public bool Matches(byte[] data, int offset)
        {
            if (offset < 0)
            {
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", SR.ValueMustBeNonNegative));
            }
            return SecurityUtils.MatchesBuffer(_identificationData, 0, data, offset);
        }

        internal string ToBase64String()
        {
            return Convert.ToBase64String(_identificationData);
        }
    }
}
