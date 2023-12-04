CREATE PROCEDURE GetBooksSortedByPublisherAuthorTitle
AS
BEGIN
    SELECT 
        b.Id,
        b.Title,
        b.Price,
		b.AuthorId,
        b.PublisherId,
        p.Name AS PublisherName,
        a.LastName AS AuthorLastName,
        a.FirstName AS AuthorFirstName
    FROM Books b
    INNER JOIN Publishers p ON b.PublisherId = p.PublisherId
    INNER JOIN Authors a ON b.AuthorId = a.AuthorId
    ORDER BY 
        p.Name, 
        a.LastName, 
        a.FirstName, 
        b.Title;
END;