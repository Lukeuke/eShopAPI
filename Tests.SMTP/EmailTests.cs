using Application.SMTP.Helpers;
using NUnit.Framework;

namespace Tests.SMTP;

public class EmailTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void ValidateEmail_Test()
    {
        Assert.AreEqual(true, EmailValidator.Validate("adam123@yahoo.com"));
        Assert.AreEqual(true, EmailValidator.Validate("jacobJohnson923@gmail.com"));
        Assert.AreEqual(false, EmailValidator.Validate("jacobJohnson923@.com"));
        Assert.AreEqual(false, EmailValidator.Validate("@gmail.com"));
    }
}