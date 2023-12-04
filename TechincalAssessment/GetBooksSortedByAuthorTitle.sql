CREATE PROCEDURE GetBooksSortedByAuthorTitle
AS
BEGIN
    SELECT 
        b.Id,
        b.Title,
        b.Price,
		b.AuthorId,
        b.PublisherId,
        a.LastName AS AuthorLastName,
        a.FirstName AS AuthorFirstName,
        p.Name AS Publisher
    FROM Books b
    INNER JOIN Authors a ON b.AuthorId = a.AuthorId
    INNER JOIN Publishers p ON b.PublisherId = p.PublisherId
    ORDER BY 
        a.LastName, 
        a.FirstName, 
        b.Title;
END;
