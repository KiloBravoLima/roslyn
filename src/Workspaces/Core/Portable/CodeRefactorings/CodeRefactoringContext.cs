﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Threading;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.CodeRefactorings
{
    /// <summary>
    /// Context for code refactorings provided by an <see cref="CodeRefactoringProvider"/>.
    /// </summary>
    public struct CodeRefactoringContext
    {
        private readonly Document _document;
        private readonly TextSpan _span;
        private readonly CancellationToken _cancellationToken;

        /// <summary>
        /// Document corresponding to the <see cref="CodeRefactoringContext.Span"/> to refactor.
        /// </summary>
        public Document Document { get { return _document; } }

        /// <summary>
        /// Text span within the <see cref="CodeRefactoringContext.Document"/> to refactor.
        /// </summary>
        public TextSpan Span { get { return _span; } }

        private readonly Action<CodeAction> _registerRefactoring;

        /// <summary>
        /// CancellationToken.
        /// </summary>
        public CancellationToken CancellationToken { get { return _cancellationToken; } }

        /// <summary>
        /// Creates a code refactoring context to be passed into <see cref="CodeRefactoringProvider.ComputeRefactoringsAsync(CodeRefactoringContext)"/> method.
        /// </summary>
        public CodeRefactoringContext(
            Document document,
            TextSpan span,
            Action<CodeAction> registerRefactoring,
            CancellationToken cancellationToken)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            if (registerRefactoring == null)
            {
                throw new ArgumentNullException(nameof(registerRefactoring));
            }

            _document = document;
            _span = span;
            _registerRefactoring = registerRefactoring;
            _cancellationToken = cancellationToken;
        }

        /// <summary>
        /// Add supplied <paramref name="action"/> to the list of refactorings that will be offered to the user.
        /// </summary>
        /// <param name="action">The <see cref="CodeAction"/> that will be invoked to apply the refactoring.</param>
        public void RegisterRefactoring(CodeAction action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            _registerRefactoring(action);
        }
    }
}
